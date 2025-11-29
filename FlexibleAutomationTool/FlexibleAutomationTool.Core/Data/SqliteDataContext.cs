using Microsoft.Data.Sqlite;
using System;

namespace FlexibleAutomationTool.Core.Data
{
    public class SqliteDataContext : IDisposable
    {
        private readonly SqliteConnection _connection;
        public SqliteConnection Connection => _connection;

        public SqliteDataContext(string dbPath)
        {
            _connection = new SqliteConnection($"Data Source={dbPath};Cache=Shared");
            _connection.Open();
            EnsureSchema();
        }

        private void EnsureSchema()
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();

            // Check if ExecutionHistory.RuleId column exists and is NOT NULL; if so, migrate to nullable
            try
            {
                using var pragma = _connection.CreateCommand();
                pragma.CommandText = "PRAGMA table_info('ExecutionHistory');";
                using var rdr = pragma.ExecuteReader();
                bool found = false;
                bool ruleIdNotNull = false;
                while (rdr.Read())
                {
                    var colName = rdr.GetString(1); // name
                    if (string.Equals(colName, "RuleId", StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        // notnull column is at index 3
                        ruleIdNotNull = rdr.GetInt32(3) != 0;
                        break;
                    }
                }

                if (found && ruleIdNotNull)
                {
                    // migrate: rename old table, create new table, copy data with NULL for RuleId where 0 or invalid
                    using var tx = _connection.BeginTransaction();
                    using var cmdRename = _connection.CreateCommand();
                    cmdRename.Transaction = tx;
                    cmdRename.CommandText = "ALTER TABLE ExecutionHistory RENAME TO ExecutionHistory_old;";
                    cmdRename.ExecuteNonQuery();

                    using var cmdCreate = _connection.CreateCommand();
                    cmdCreate.Transaction = tx;
                    cmdCreate.CommandText = @"CREATE TABLE IF NOT EXISTS ExecutionHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    RuleId INTEGER,
    ExecutedAt TEXT NOT NULL,
    Status TEXT,
    Message TEXT,
    FOREIGN KEY (RuleId) REFERENCES Rules(Id) ON DELETE CASCADE
);";
                    cmdCreate.ExecuteNonQuery();

                    using var cmdCopy = _connection.CreateCommand();
                    cmdCopy.Transaction = tx;
                    // convert invalid or zero RuleId to NULL to satisfy foreign key
                    cmdCopy.CommandText = @"INSERT INTO ExecutionHistory (RuleId, ExecutedAt, Status, Message)
SELECT CASE
         WHEN RuleId IS NULL THEN NULL
         WHEN RuleId = 0 THEN NULL
         WHEN NOT EXISTS(SELECT 1 FROM Rules WHERE Id = ExecutionHistory_old.RuleId) THEN NULL
         ELSE RuleId END,
       ExecutedAt, Status, Message
FROM ExecutionHistory_old;";
                    cmdCopy.ExecuteNonQuery();

                    using var cmdDrop = _connection.CreateCommand();
                    cmdDrop.Transaction = tx;
                    cmdDrop.CommandText = "DROP TABLE ExecutionHistory_old;";
                    cmdDrop.ExecuteNonQuery();

                    tx.Commit();
                }
            }
            catch
            {
                // ignore migration errors and continue ensuring schema below
            }

            using var cmd2 = _connection.CreateCommand();
            cmd2.CommandText = @"-- Triggers
CREATE TABLE IF NOT EXISTS Triggers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    TriggerType TEXT NOT NULL,
    Schedule TEXT,
    EventSource TEXT,
    Condition TEXT
);

-- Actions
CREATE TABLE IF NOT EXISTS Actions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ActionType TEXT NOT NULL,
    ServiceType TEXT,
    Command TEXT,
    Parameters TEXT
);

-- Rules
CREATE TABLE IF NOT EXISTS Rules (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    TriggerId INTEGER NOT NULL,
    ActionId INTEGER NOT NULL,
    IsActive INTEGER NOT NULL CHECK (IsActive IN (0, 1)),
    FOREIGN KEY (TriggerId) REFERENCES Triggers(Id) ON DELETE CASCADE,
    FOREIGN KEY (ActionId) REFERENCES Actions(Id) ON DELETE CASCADE
);

-- ExecutionHistory
CREATE TABLE IF NOT EXISTS ExecutionHistory (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    RuleId INTEGER,
    ExecutedAt TEXT NOT NULL,
    Status TEXT,
    Message TEXT,
    FOREIGN KEY (RuleId) REFERENCES Rules(Id) ON DELETE CASCADE
);

-- Services
CREATE TABLE IF NOT EXISTS Services (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    ServiceType TEXT NOT NULL,
    Config TEXT
);

-- MacroActionItems
CREATE TABLE IF NOT EXISTS MacroActionItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MacroActionId INTEGER NOT NULL,
    ChildActionId INTEGER NOT NULL,
    OrderIndex INTEGER NOT NULL,
    FOREIGN KEY (MacroActionId) REFERENCES Actions(Id) ON DELETE CASCADE,
    FOREIGN KEY (ChildActionId) REFERENCES Actions(Id) ON DELETE CASCADE
);";
            cmd2.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
