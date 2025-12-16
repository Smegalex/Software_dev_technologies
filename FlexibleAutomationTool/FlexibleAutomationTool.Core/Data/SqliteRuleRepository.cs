using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;

namespace FlexibleAutomationTool.Core.Data
{
    public class SqliteRuleRepository : IRuleRepository, IDisposable
    {
        private readonly SqliteDataContext _context;
        private readonly object _lock = new();

        public SqliteRuleRepository(SqliteDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Rule> GetAll()
        {
            var list = new List<Rule>();
            lock (_lock)
            {
                using var cmd = _context.Connection.CreateCommand();
                cmd.CommandText = @"SELECT r.Id, r.Name, r.Description, r.IsActive,
t.TriggerType, t.Schedule, t.EventSource, t.Condition,
a.ActionType, a.ServiceType, a.Command, a.Parameters, a.Id as ActionId
FROM Rules r
JOIN Triggers t ON r.TriggerId = t.Id
JOIN Actions a ON r.ActionId = a.Id
ORDER BY r.Id";
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = rdr.GetInt32(0);
                    var name = rdr.GetString(1);
                    var desc = rdr.IsDBNull(2) ? null : rdr.GetString(2);
                    var isActive = rdr.GetInt32(3) != 0;

                    var triggerType = rdr.IsDBNull(4) ? null : rdr.GetString(4);
                    var schedule = rdr.IsDBNull(5) ? null : rdr.GetString(5);
                    var eventSource = rdr.IsDBNull(6) ? null : rdr.GetString(6);
                    var condition = rdr.IsDBNull(7) ? null : rdr.GetString(7);

                    var actionType = rdr.IsDBNull(8) ? null : rdr.GetString(8);
                    var serviceType = rdr.IsDBNull(9) ? null : rdr.GetString(9);
                    var command = rdr.IsDBNull(10) ? null : rdr.GetString(10);
                    var parameters = rdr.IsDBNull(11) ? null : rdr.GetString(11);
                    var actionId = rdr.IsDBNull(12) ? 0 : rdr.GetInt32(12);

                    var rule = new Rule { Id = id, Name = name, Description = desc, IsActive = isActive };

                    // Reconstruct trigger
                    if (string.Equals(triggerType, "TimeTrigger", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(schedule))
                    {
                        var parts = schedule.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[0], out var hour) && int.TryParse(parts[1], out var minute))
                            rule.Trigger = new FlexibleAutomationTool.Core.Triggers.TimeTrigger { Hour = hour, Minute = minute };
                        else
                            rule.Trigger = new FlexibleAutomationTool.Core.Triggers.EventTrigger(eventSource, null);
                    }
                    else if (string.Equals(triggerType, "EventTrigger", StringComparison.OrdinalIgnoreCase))
                    {
                        rule.Trigger = new FlexibleAutomationTool.Core.Triggers.EventTrigger(eventSource, null);
                    }
                    else
                    {
                        rule.Trigger = new FlexibleAutomationTool.Core.Triggers.EventTrigger(null, null);
                    }

                    // Reconstruct action
                    if (string.Equals(actionType, "MessageBox", StringComparison.OrdinalIgnoreCase))
                    {
                        rule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction { Title = command ?? string.Empty, Message = parameters ?? string.Empty };
                    }
                    else if (string.Equals(actionType, "RunProgram", StringComparison.OrdinalIgnoreCase))
                    {
                        rule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.RunProgramAction { Path = command ?? string.Empty, Arguments = parameters };
                    }
                    else if (string.Equals(actionType, "OpenUrl", StringComparison.OrdinalIgnoreCase))
                    {
                        rule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction { Url = command ?? string.Empty };
                    }
                    else if (string.Equals(actionType, "FileWrite", StringComparison.OrdinalIgnoreCase))
                    {
                        rule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction { FilePath = command ?? string.Empty, Content = parameters ?? string.Empty };
                    }
                    else if (string.Equals(actionType, "Macro", StringComparison.OrdinalIgnoreCase) && actionId != 0)
                    {
                        // Reconstruct macro by querying MacroActionItems and child Actions
                        var macro = new FlexibleAutomationTool.Core.Actions.MacroAction();
                        using (var cmdItems = _context.Connection.CreateCommand())
                        {
                            cmdItems.CommandText = "SELECT ChildActionId FROM MacroActionItems WHERE MacroActionId=$mid ORDER BY OrderIndex";
                            cmdItems.Parameters.AddWithValue("$mid", actionId);
                            using var rdrItems = cmdItems.ExecuteReader();
                            while (rdrItems.Read())
                            {
                                var childId = rdrItems.GetInt32(0);
                                // load child action
                                using var cmdChild = _context.Connection.CreateCommand();
                                cmdChild.CommandText = "SELECT ActionType, ServiceType, Command, Parameters FROM Actions WHERE Id=$id";
                                cmdChild.Parameters.AddWithValue("$id", childId);
                                using var rdrChild = cmdChild.ExecuteReader();
                                if (rdrChild.Read())
                                {
                                    var cType = rdrChild.IsDBNull(0) ? null : rdrChild.GetString(0);
                                    var cService = rdrChild.IsDBNull(1) ? null : rdrChild.GetString(1);
                                    var cCommand = rdrChild.IsDBNull(2) ? null : rdrChild.GetString(2);
                                    var cParams = rdrChild.IsDBNull(3) ? null : rdrChild.GetString(3);

                                    if (string.Equals(cType, "MessageBox", StringComparison.OrdinalIgnoreCase))
                                        macro.Actions.Add(new FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction { Title = cCommand ?? string.Empty, Message = cParams ?? string.Empty });
                                    else if (string.Equals(cType, "RunProgram", StringComparison.OrdinalIgnoreCase))
                                        macro.Actions.Add(new FlexibleAutomationTool.Core.Actions.InternalActions.RunProgramAction { Path = cCommand ?? string.Empty, Arguments = cParams });
                                    else if (string.Equals(cType, "OpenUrl", StringComparison.OrdinalIgnoreCase))
                                        macro.Actions.Add(new FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction { Url = cCommand ?? string.Empty });
                                    else if (string.Equals(cType, "FileWrite", StringComparison.OrdinalIgnoreCase))
                                        macro.Actions.Add(new FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction { FilePath = cCommand ?? string.Empty, Content = cParams ?? string.Empty });
                                    else
                                        macro.Actions.Add(new FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction { Title = string.Empty, Message = string.Empty });
                                }
                            }
                        }

                        rule.Action = macro;
                    }
                    else
                    {
                        // Default to MessageBoxAction placeholder to avoid nulls
                        rule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction { Title = string.Empty, Message = string.Empty };
                    }

                    list.Add(rule);
                }
            }

            return list;
        }

        private long InsertTrigger(Rule rule, SqliteTransaction tx)
        {
            using var cmdT = _context.Connection.CreateCommand();
            cmdT.Transaction = tx;
            cmdT.CommandText = "INSERT INTO Triggers (TriggerType, Schedule, EventSource, Condition) VALUES ($tt,$sch,$es,$cond); SELECT last_insert_rowid();";
            if (rule.Trigger is FlexibleAutomationTool.Core.Triggers.TimeTrigger tt)
            {
                cmdT.Parameters.AddWithValue("$tt", "TimeTrigger");
                cmdT.Parameters.AddWithValue("$sch", $"{tt.Hour}:{tt.Minute}");
                cmdT.Parameters.AddWithValue("$es", (object)DBNull.Value);
                cmdT.Parameters.AddWithValue("$cond", (object)DBNull.Value);
            }
            else if (rule.Trigger is FlexibleAutomationTool.Core.Triggers.EventTrigger et)
            {
                cmdT.Parameters.AddWithValue("$tt", "EventTrigger");
                cmdT.Parameters.AddWithValue("$sch", (object)DBNull.Value);
                cmdT.Parameters.AddWithValue("$es", et.EventSource?.ToString() ?? (object)DBNull.Value);
                cmdT.Parameters.AddWithValue("$cond", et.Condition == null ? (object)DBNull.Value : et.Condition.ToString());
            }
            else
            {
                cmdT.Parameters.AddWithValue("$tt", rule.Trigger?.GetType().Name ?? "Unknown");
                cmdT.Parameters.AddWithValue("$sch", (object)DBNull.Value);
                cmdT.Parameters.AddWithValue("$es", (object)DBNull.Value);
                cmdT.Parameters.AddWithValue("$cond", (object)DBNull.Value);
            }

            return (long)cmdT.ExecuteScalar();
        }

        private long InsertAction(Rule rule, SqliteTransaction tx)
        {
            // Support MacroAction by inserting an action entry representing macro and child actions
            if (rule.Action is FlexibleAutomationTool.Core.Actions.MacroAction mac)
            {
                // Insert placeholder Macro action record
                using var cmdA = _context.Connection.CreateCommand();
                cmdA.Transaction = tx;
                cmdA.CommandText = "INSERT INTO Actions (ActionType, ServiceType, Command, Parameters) VALUES ($at,$st,$cm,$pm); SELECT last_insert_rowid();";
                cmdA.Parameters.AddWithValue("$at", "Macro");
                cmdA.Parameters.AddWithValue("$st", (object)DBNull.Value);
                cmdA.Parameters.AddWithValue("$cm", (object)DBNull.Value);
                cmdA.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                var macroId = (long)cmdA.ExecuteScalar();

                // Insert child actions and MacroActionItems
                int order = 0;
                foreach (var child in mac.Actions)
                {
                    // insert child action
                    using var cmdChild = _context.Connection.CreateCommand();
                    cmdChild.Transaction = tx;
                    cmdChild.CommandText = "INSERT INTO Actions (ActionType, ServiceType, Command, Parameters) VALUES ($at,$st,$cm,$pm); SELECT last_insert_rowid();";

                    if (child is FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction cma)
                    {
                        cmdChild.Parameters.AddWithValue("$at", "MessageBox");
                        cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                        cmdChild.Parameters.AddWithValue("$cm", cma.Title ?? string.Empty);
                        cmdChild.Parameters.AddWithValue("$pm", cma.Message ?? string.Empty);
                    }
                    else if (child is FlexibleAutomationTool.Core.Actions.InternalActions.RunProgramAction cra)
                    {
                        cmdChild.Parameters.AddWithValue("$at", "RunProgram");
                        cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                        cmdChild.Parameters.AddWithValue("$cm", cra.Path ?? string.Empty);
                        cmdChild.Parameters.AddWithValue("$pm", cra.Arguments ?? (object)DBNull.Value);
                    }
                    else if (child is FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction cua)
                    {
                        cmdChild.Parameters.AddWithValue("$at", "OpenUrl");
                        cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                        cmdChild.Parameters.AddWithValue("$cm", cua.Url ?? string.Empty);
                        cmdChild.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                    }
                    else if (child is FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction cfa)
                    {
                        cmdChild.Parameters.AddWithValue("$at", "FileWrite");
                        cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                        cmdChild.Parameters.AddWithValue("$cm", cfa.FilePath ?? string.Empty);
                        cmdChild.Parameters.AddWithValue("$pm", cfa.Content ?? string.Empty);
                    }
                    else
                    {
                        cmdChild.Parameters.AddWithValue("$at", child?.GetType().Name ?? "Unknown");
                        cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                        cmdChild.Parameters.AddWithValue("$cm", (object)DBNull.Value);
                        cmdChild.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                    }

                    var childId = (long)cmdChild.ExecuteScalar();

                    using var cmdItem = _context.Connection.CreateCommand();
                    cmdItem.Transaction = tx;
                    cmdItem.CommandText = "INSERT INTO MacroActionItems (MacroActionId, ChildActionId, OrderIndex) VALUES ($mid,$cid,$ord);";
                    cmdItem.Parameters.AddWithValue("$mid", macroId);
                    cmdItem.Parameters.AddWithValue("$cid", childId);
                    cmdItem.Parameters.AddWithValue("$ord", order++);
                    cmdItem.ExecuteNonQuery();
                }

                return macroId;
            }

            using var cmdA2 = _context.Connection.CreateCommand();
            cmdA2.Transaction = tx;
            cmdA2.CommandText = "INSERT INTO Actions (ActionType, ServiceType, Command, Parameters) VALUES ($at,$st,$cm,$pm); SELECT last_insert_rowid();";
            if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction ma)
            {
                cmdA2.Parameters.AddWithValue("$at", "MessageBox");
                cmdA2.Parameters.AddWithValue("$st", (object)DBNull.Value);
                cmdA2.Parameters.AddWithValue("$cm", ma.Title ?? string.Empty);
                cmdA2.Parameters.AddWithValue("$pm", ma.Message ?? string.Empty);
            }
            else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.RunProgramAction ra)
            {
                cmdA2.Parameters.AddWithValue("$at", "RunProgram");
                cmdA2.Parameters.AddWithValue("$st", (object)DBNull.Value);
                cmdA2.Parameters.AddWithValue("$cm", ra.Path ?? string.Empty);
                cmdA2.Parameters.AddWithValue("$pm", ra.Arguments ?? (object)DBNull.Value);
            }
            else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction ua)
            {
                cmdA2.Parameters.AddWithValue("$at", "OpenUrl");
                cmdA2.Parameters.AddWithValue("$st", (object)DBNull.Value);
                cmdA2.Parameters.AddWithValue("$cm", ua.Url ?? string.Empty);
                cmdA2.Parameters.AddWithValue("$pm", (object)DBNull.Value);
            }
            else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction fa)
            {
                cmdA2.Parameters.AddWithValue("$at", "FileWrite");
                cmdA2.Parameters.AddWithValue("$st", (object)DBNull.Value);
                cmdA2.Parameters.AddWithValue("$cm", fa.FilePath ?? string.Empty);
                cmdA2.Parameters.AddWithValue("$pm", fa.Content ?? string.Empty);
            }
            else
            {
                cmdA2.Parameters.AddWithValue("$at", rule.Action?.GetType().Name ?? "Unknown");
                cmdA2.Parameters.AddWithValue("$st", (object)DBNull.Value);
                cmdA2.Parameters.AddWithValue("$cm", (object)DBNull.Value);
                cmdA2.Parameters.AddWithValue("$pm", (object)DBNull.Value);
            }

            return (long)cmdA2.ExecuteScalar();
        }

        public void Add(Rule rule)
        {
            lock (_lock)
            {
                using var tx = _context.Connection.BeginTransaction();

                var triggerId = InsertTrigger(rule, tx);
                var actionId = InsertAction(rule, tx);

                using var cmdR = _context.Connection.CreateCommand();
                cmdR.Transaction = tx;
                cmdR.CommandText = "INSERT INTO Rules (Name, Description, TriggerId, ActionId, IsActive) VALUES ($name,$desc,$tid,$aid,$ia); SELECT last_insert_rowid();";
                cmdR.Parameters.AddWithValue("$name", rule.Name);
                cmdR.Parameters.AddWithValue("$desc", rule.Description ?? (object)DBNull.Value);
                cmdR.Parameters.AddWithValue("$tid", triggerId);
                cmdR.Parameters.AddWithValue("$aid", actionId);
                cmdR.Parameters.AddWithValue("$ia", rule.IsActive ? 1 : 0);

                var id = (long)cmdR.ExecuteScalar();
                rule.Id = (int)id;

                tx.Commit();
            }
        }

        public void Update(Rule rule)
        {
            lock (_lock)
            {
                using var tx = _context.Connection.BeginTransaction();

                // Find triggerId and actionId for rule
                long triggerId = 0, actionId = 0;
                using (var cmdFind = _context.Connection.CreateCommand())
                {
                    cmdFind.CommandText = "SELECT TriggerId, ActionId FROM Rules WHERE Id=$id";
                    cmdFind.Parameters.AddWithValue("$id", rule.Id);
                    using var rdr = cmdFind.ExecuteReader();
                    if (rdr.Read())
                    {
                        triggerId = rdr.IsDBNull(0) ? 0 : rdr.GetInt64(0);
                        actionId = rdr.IsDBNull(1) ? 0 : rdr.GetInt64(1);
                    }
                }

                if (triggerId != 0)
                {
                    using var cmdT = _context.Connection.CreateCommand();
                    cmdT.Transaction = tx;
                    cmdT.CommandText = "UPDATE Triggers SET TriggerType=$tt, Schedule=$sch, EventSource=$es, Condition=$cond WHERE Id=$id";
                    if (rule.Trigger is FlexibleAutomationTool.Core.Triggers.TimeTrigger tt)
                    {
                        cmdT.Parameters.AddWithValue("$tt", "TimeTrigger");
                        cmdT.Parameters.AddWithValue("$sch", $"{tt.Hour}:{tt.Minute}");
                        cmdT.Parameters.AddWithValue("$es", (object)DBNull.Value);
                        cmdT.Parameters.AddWithValue("$cond", (object)DBNull.Value);
                    }
                    else if (rule.Trigger is FlexibleAutomationTool.Core.Triggers.EventTrigger et)
                    {
                        cmdT.Parameters.AddWithValue("$tt", "EventTrigger");
                        cmdT.Parameters.AddWithValue("$sch", (object)DBNull.Value);
                        cmdT.Parameters.AddWithValue("$es", et.EventSource?.ToString() ?? (object)DBNull.Value);
                        cmdT.Parameters.AddWithValue("$cond", et.Condition == null ? (object)DBNull.Value : et.Condition.ToString());
                    }
                    else
                    {
                        cmdT.Parameters.AddWithValue("$tt", rule.Trigger?.GetType().Name ?? "Unknown");
                        cmdT.Parameters.AddWithValue("$sch", (object)DBNull.Value);
                        cmdT.Parameters.AddWithValue("$es", (object)DBNull.Value);
                        cmdT.Parameters.AddWithValue("$cond", (object)DBNull.Value);
                    }
                    cmdT.Parameters.AddWithValue("$id", triggerId);
                    cmdT.ExecuteNonQuery();
                }

                if (actionId != 0)
                {
                    // Handle MacroAction updates: ensure ActionType stays as 'Macro', remove old child actions/items and insert new ones
                    if (rule.Action is FlexibleAutomationTool.Core.Actions.MacroAction mac)
                    {
                        // Update main action row to be Macro
                        using var cmdAUpdate = _context.Connection.CreateCommand();
                        cmdAUpdate.Transaction = tx;
                        cmdAUpdate.CommandText = "UPDATE Actions SET ActionType=$at, ServiceType=$st, Command=$cm, Parameters=$pm WHERE Id=$id";
                        cmdAUpdate.Parameters.AddWithValue("$at", "Macro");
                        cmdAUpdate.Parameters.AddWithValue("$st", (object)DBNull.Value);
                        cmdAUpdate.Parameters.AddWithValue("$cm", (object)DBNull.Value);
                        cmdAUpdate.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                        cmdAUpdate.Parameters.AddWithValue("$id", actionId);
                        cmdAUpdate.ExecuteNonQuery();

                        // Collect existing child action ids
                        var existingChildIds = new List<long>();
                        using (var cmdGetItems = _context.Connection.CreateCommand())
                        {
                            cmdGetItems.Transaction = tx;
                            cmdGetItems.CommandText = "SELECT ChildActionId FROM MacroActionItems WHERE MacroActionId=$mid";
                            cmdGetItems.Parameters.AddWithValue("$mid", actionId);
                            using var rdr = cmdGetItems.ExecuteReader();
                            while (rdr.Read())
                                existingChildIds.Add(rdr.GetInt64(0));
                        }

                        // Delete MacroActionItems for this macro
                        using (var cmdDelItems = _context.Connection.CreateCommand())
                        {
                            cmdDelItems.Transaction = tx;
                            cmdDelItems.CommandText = "DELETE FROM MacroActionItems WHERE MacroActionId=$mid";
                            cmdDelItems.Parameters.AddWithValue("$mid", actionId);
                            cmdDelItems.ExecuteNonQuery();
                        }

                        // Delete old child action rows
                        if (existingChildIds.Count > 0)
                        {
                            // build a parameterized IN clause
                            var idx = 0;
                            var sb = new System.Text.StringBuilder();
                            foreach (var cid in existingChildIds)
                            {
                                if (idx > 0) sb.Append(',');
                                sb.Append("$cid" + idx);
                                idx++;
                            }

                            using var cmdDelChildren = _context.Connection.CreateCommand();
                            cmdDelChildren.Transaction = tx;
                            cmdDelChildren.CommandText = $"DELETE FROM Actions WHERE Id IN ({sb})";
                            idx = 0;
                            foreach (var cid in existingChildIds)
                            {
                                cmdDelChildren.Parameters.AddWithValue("$cid" + (idx++), cid);
                            }
                            cmdDelChildren.ExecuteNonQuery();
                        }

                        // Insert new child actions and MacroActionItems
                        int order = 0;
                        foreach (var child in mac.Actions)
                        {
                            using var cmdChild = _context.Connection.CreateCommand();
                            cmdChild.Transaction = tx;
                            cmdChild.CommandText = "INSERT INTO Actions (ActionType, ServiceType, Command, Parameters) VALUES ($at,$st,$cm,$pm); SELECT last_insert_rowid();";

                            if (child is FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction cma)
                            {
                                cmdChild.Parameters.AddWithValue("$at", "MessageBox");
                                cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                                cmdChild.Parameters.AddWithValue("$cm", cma.Title ?? string.Empty);
                                cmdChild.Parameters.AddWithValue("$pm", cma.Message ?? string.Empty);
                            }
                            else if (child is FlexibleAutomationTool.Core.Actions.InternalActions.RunProgramAction cra)
                            {
                                cmdChild.Parameters.AddWithValue("$at", "RunProgram");
                                cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                                cmdChild.Parameters.AddWithValue("$cm", cra.Path ?? string.Empty);
                                cmdChild.Parameters.AddWithValue("$pm", cra.Arguments ?? (object)DBNull.Value);
                            }
                            else if (child is FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction cua)
                            {
                                cmdChild.Parameters.AddWithValue("$at", "OpenUrl");
                                cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                                cmdChild.Parameters.AddWithValue("$cm", cua.Url ?? string.Empty);
                                cmdChild.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                            }
                            else if (child is FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction cfa)
                            {
                                cmdChild.Parameters.AddWithValue("$at", "FileWrite");
                                cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                                cmdChild.Parameters.AddWithValue("$cm", cfa.FilePath ?? string.Empty);
                                cmdChild.Parameters.AddWithValue("$pm", cfa.Content ?? string.Empty);
                            }
                            else
                            {
                                cmdChild.Parameters.AddWithValue("$at", child?.GetType().Name ?? "Unknown");
                                cmdChild.Parameters.AddWithValue("$st", (object)DBNull.Value);
                                cmdChild.Parameters.AddWithValue("$cm", (object)DBNull.Value);
                                cmdChild.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                            }

                            var childId = (long)cmdChild.ExecuteScalar();

                            using var cmdItem = _context.Connection.CreateCommand();
                            cmdItem.Transaction = tx;
                            cmdItem.CommandText = "INSERT INTO MacroActionItems (MacroActionId, ChildActionId, OrderIndex) VALUES ($mid,$cid,$ord);";
                            cmdItem.Parameters.AddWithValue("$mid", actionId);
                            cmdItem.Parameters.AddWithValue("$cid", childId);
                            cmdItem.Parameters.AddWithValue("$ord", order++);
                            cmdItem.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using var cmdA = _context.Connection.CreateCommand();
                        cmdA.Transaction = tx;
                        cmdA.CommandText = "UPDATE Actions SET ActionType=$at, ServiceType=$st, Command=$cm, Parameters=$pm WHERE Id=$id";
                        if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction ma)
                        {
                            cmdA.Parameters.AddWithValue("$at", "MessageBox");
                            cmdA.Parameters.AddWithValue("$st", (object)DBNull.Value);
                            cmdA.Parameters.AddWithValue("$cm", ma.Title ?? string.Empty);
                            cmdA.Parameters.AddWithValue("$pm", ma.Message ?? string.Empty);
                        }
                        else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.RunProgramAction ra)
                        {
                            cmdA.Parameters.AddWithValue("$at", "RunProgram");
                            cmdA.Parameters.AddWithValue("$st", (object)DBNull.Value);
                            cmdA.Parameters.AddWithValue("$cm", ra.Path ?? string.Empty);
                            cmdA.Parameters.AddWithValue("$pm", ra.Arguments ?? (object)DBNull.Value);
                        }
                        else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction ua)
                        {
                            cmdA.Parameters.AddWithValue("$at", "OpenUrl");
                            cmdA.Parameters.AddWithValue("$st", (object)DBNull.Value);
                            cmdA.Parameters.AddWithValue("$cm", ua.Url ?? string.Empty);
                            cmdA.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                        }
                        else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction fa)
                        {
                            cmdA.Parameters.AddWithValue("$at", "FileWrite");
                            cmdA.Parameters.AddWithValue("$st", (object)DBNull.Value);
                            cmdA.Parameters.AddWithValue("$cm", fa.FilePath ?? string.Empty);
                            cmdA.Parameters.AddWithValue("$pm", fa.Content ?? string.Empty);
                        }
                        else
                        {
                            cmdA.Parameters.AddWithValue("$at", rule.Action?.GetType().Name ?? "Unknown");
                            cmdA.Parameters.AddWithValue("$st", (object)DBNull.Value);
                            cmdA.Parameters.AddWithValue("$cm", (object)DBNull.Value);
                            cmdA.Parameters.AddWithValue("$pm", (object)DBNull.Value);
                        }
                        cmdA.Parameters.AddWithValue("$id", actionId);
                        cmdA.ExecuteNonQuery();
                    }
                }

                using var cmdR = _context.Connection.CreateCommand();
                cmdR.Transaction = tx;
                cmdR.CommandText = "UPDATE Rules SET Name=$name, Description=$desc, IsActive=$ia WHERE Id=$id";
                cmdR.Parameters.AddWithValue("$name", rule.Name);
                cmdR.Parameters.AddWithValue("$desc", rule.Description ?? (object)DBNull.Value);
                cmdR.Parameters.AddWithValue("$ia", rule.IsActive ? 1 : 0);
                cmdR.Parameters.AddWithValue("$id", rule.Id);
                cmdR.ExecuteNonQuery();

                tx.Commit();
            }
        }

        public void Delete(int id)
        {
            using var cmd = _context.Connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Rules WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
