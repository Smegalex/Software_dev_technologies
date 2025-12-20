using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using System.Diagnostics;

namespace FlexibleAutomationTool.Core.Data
{
    public class SqliteLogger : IExecutionHistoryRepository, IDisposable
    {
        private readonly SqliteDataContext _context;
        private readonly object _lock = new();
        private readonly bool _ruleIdNotNull;
        private readonly int? _systemRuleId;

        public SqliteLogger(SqliteDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // detect if ExecutionHistory.RuleId column is NOT NULL in this DB
            try
            {
                using var cmd = _context.Connection.CreateCommand();
                cmd.CommandText = "PRAGMA table_info('ExecutionHistory');";
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var colName = rdr.GetString(1);
                    if (string.Equals(colName, "RuleId", StringComparison.OrdinalIgnoreCase))
                    {
                        _ruleIdNotNull = rdr.GetInt32(3) != 0; // notnull column
                        break;
                    }
                }
            }
            catch
            {
                _ruleIdNotNull = false;
            }

            // If DB requires RuleId to be NOT NULL, try to locate the system placeholder rule created by the data context
            if (_ruleIdNotNull)
            {
                try
                {
                    using var cmd = _context.Connection.CreateCommand();
                    cmd.CommandText = "SELECT Id FROM Rules WHERE Name=$name LIMIT 1";
                    cmd.Parameters.AddWithValue("$name", "__System__");
                    var res = cmd.ExecuteScalar();
                    if (res != null && res != DBNull.Value)
                    {
                        // SQLite integers come back as long
                        _systemRuleId = Convert.ToInt32(res);
                    }
                }
                catch
                {
                    _systemRuleId = null;
                }
            }
        }

        public void Add(int ruleId, DateTime executedAt, string status, string message)
        {
            lock (_lock)
            {
                try
                {
                    // Local copy of rule id we'll use for insertion
                    var insertRuleId = ruleId;

                    // If DB schema requires RuleId NOT NULL then map non-rule entries (ruleId <= 0)
                    // to the system placeholder rule if available; otherwise skip.
                    if (insertRuleId <= 0 && _ruleIdNotNull)
                    {
                        if (_systemRuleId.HasValue)
                        {
                            insertRuleId = _systemRuleId.Value;
                        }
                        else
                        {
                            Debug.WriteLine($"SqliteLogger: skipped non-rule history entry (ruleId={ruleId}) because ExecutionHistory.RuleId is NOT NULL and no system placeholder rule available.");
                            return;
                        }
                    }

                    using var cmd = _context.Connection.CreateCommand();
                    if (insertRuleId <= 0)
                    {
                        // omit RuleId column when DB allows NULL
                        cmd.CommandText = "INSERT INTO ExecutionHistory (ExecutedAt, Status, Message) VALUES ($ts,$st,$msg);";
                        cmd.Parameters.AddWithValue("$ts", executedAt.ToString("o"));
                        cmd.Parameters.AddWithValue("$st", status ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("$msg", message ?? (object)DBNull.Value);
                    }
                    else
                    {
                        // Ensure referenced Rule exists to avoid FK violations
                        using (var chk = _context.Connection.CreateCommand())
                        {
                            chk.CommandText = "SELECT 1 FROM Rules WHERE Id=$rid LIMIT 1";
                            chk.Parameters.AddWithValue("$rid", insertRuleId);
                            var exists = chk.ExecuteScalar();
                            if (exists == null)
                            {
                                // If referenced rule doesn't exist, try to fallback to system placeholder (if DB requires NOT NULL)
                                Debug.WriteLine($"SqliteLogger: referenced rule with Id={insertRuleId} was not found.");

                                if (_ruleIdNotNull && _systemRuleId.HasValue)
                                {
                                    insertRuleId = _systemRuleId.Value;
                                    Debug.WriteLine($"SqliteLogger: falling back to system placeholder rule Id={insertRuleId} for history entry.");
                                }
                                else if (_ruleIdNotNull && !_systemRuleId.HasValue)
                                {
                                    Debug.WriteLine($"SqliteLogger: cannot insert FK entry because ExecutionHistory.RuleId is NOT NULL and no system placeholder exists; insert skipped for ruleId={ruleId}.");
                                    return;
                                }
                                else
                                {
                                    // DB allows NULL, insert without RuleId
                                    cmd.CommandText = "INSERT INTO ExecutionHistory (ExecutedAt, Status, Message) VALUES ($ts,$st,$msg);";
                                    cmd.Parameters.AddWithValue("$ts", executedAt.ToString("o"));
                                    cmd.Parameters.AddWithValue("$st", status ?? (object)DBNull.Value);
                                    cmd.Parameters.AddWithValue("$msg", message ?? (object)DBNull.Value);
                                    cmd.ExecuteNonQuery();
                                    return;
                                }
                            }
                        }

                        cmd.CommandText = "INSERT INTO ExecutionHistory (RuleId, ExecutedAt, Status, Message) VALUES ($rid,$ts,$st,$msg);";
                        cmd.Parameters.AddWithValue("$rid", insertRuleId);
                        cmd.Parameters.AddWithValue("$ts", executedAt.ToString("o"));
                        cmd.Parameters.AddWithValue("$st", status ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("$msg", message ?? (object)DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    // Swallow SQLite write errors to avoid breaking UI; log for diagnostics
                    Debug.WriteLine($"SqliteLogger.Add failed: {ex.SqliteErrorCode} - {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"SqliteLogger.Add unexpected error: {ex}");
                }
            }
        }

        public IEnumerable<LogEntry> GetAll()
        {
            var list = new List<LogEntry>();
            lock (_lock)
            {
                using var cmd = _context.Connection.CreateCommand();
                cmd.CommandText = "SELECT Id, RuleId, ExecutedAt, Status, Message FROM ExecutionHistory ORDER BY Id";
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = rdr.GetInt32(0);
                    int? ruleId = rdr.IsDBNull(1) ? null : rdr.GetInt32(1);
                    var ts = DateTime.Parse(rdr.GetString(2));
                    var status = rdr.IsDBNull(3) ? null : rdr.GetString(3);
                    var message = rdr.IsDBNull(4) ? null : rdr.GetString(4);
                    list.Add(new LogEntry { Id = id, Timestamp = ts, RuleName = ruleId?.ToString() ?? string.Empty, Message = message ?? status ?? string.Empty });
                }
            }
            return list;
        }

        public IEnumerable<LogEntry> GetByRuleId(int ruleId)
        {
            var list = new List<LogEntry>();
            lock (_lock)
            {
                using var cmd = _context.Connection.CreateCommand();
                cmd.CommandText = "SELECT Id, RuleId, ExecutedAt, Status, Message FROM ExecutionHistory WHERE RuleId=$rid ORDER BY Id";
                cmd.Parameters.AddWithValue("$rid", ruleId);
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = rdr.GetInt32(0);
                    var ts = DateTime.Parse(rdr.GetString(2));
                    var status = rdr.IsDBNull(3) ? null : rdr.GetString(3);
                    var message = rdr.IsDBNull(4) ? null : rdr.GetString(4);
                    list.Add(new LogEntry { Id = id, Timestamp = ts, RuleName = ruleId.ToString(), Message = message ?? status ?? string.Empty });
                }
            }
            return list;
        }

        public void Dispose()
        {
            // context is owned by DI, do not dispose here
        }
    }
}
