using BudgetCLI.Data.Models;
using Microsoft.Data.Sqlite;

namespace BudgetCLI.Data
{
    public class MigrationManager
    {
        static string MigrationScriptsPath = "./Data/MigrationScripts/";
        static OrderedDictionary<string, string> DBVersionStringToMigrationScript = new()
        {
            {"0.1", "CreateOneTimeBillsTable.sql"},
            {"0.2", "CreateRecurringBillsTable.sql"},
            {"0.3", "CreateBudgetJobsTable.sql"},
            {"0.3.1", "RecurringBillNameFix.sql"}
        };
        SqliteConnection Connection;
        AppInfoModel AppInfo = new();
        public MigrationManager()
        {
            Connection = DatabaseHelper.GetReadWriteConnection();
        }

        public void DoMigrations()
        {
            DatabaseHelper.EnsureDatabaseExists();
            Connection.Open();
            if (!IsDatabaseInitialized())
            {
                ReadAndRunSqlScript("BudgetDatabaseCreation.sql");
            }
            UpdateLastOpened();
            GetAppInfo();
            Migrate();
            Connection.Close();
        }

        bool IsDatabaseInitialized()
        {
            bool result;
            try
            {
                SqliteCommand command = Connection.CreateCommand();
                command.CommandText = "SELECT * FROM sqlite_master;";
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (SqliteException)
            {
                result = false;
            }
            return result;
        }

        void GetAppInfo()
        {
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM AppInfo";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                switch (reader.GetString(0))
                {
                    case "AppVersion":
                        AppInfo.AppVersion = reader.GetString(1);
                        break;
                    case "DatabaseVersion":
                        AppInfo.DatabaseVersion = reader.GetString(1);
                        break;
                    case "LastUpdate":
                        AppInfo.LastUpdated = DateOnly.FromDateTime(reader.GetDateTime(1));
                        break;
                    case "LastOpened":
                        AppInfo.LastOpened = DateOnly.FromDateTime(reader.GetDateTime(1));
                        break;
                }
            }
        }

        void UpdateLastOpened()
        {
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = """
                UPDATE AppInfo
                SET InfoValue = $today
                WHERE InfoKey = 'LastOpened';
            """;
            command.Parameters.AddWithValue("$today", DateOnly.FromDateTime(DateTime.Today));
            command.ExecuteNonQuery();
        }

        void UpdateLastUpdated()
        {
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = """
                UPDATE AppInfo
                SET InfoValue = $today
                WHERE InfoKey = 'LastUpdated';
            """;
            command.Parameters.AddWithValue("$today", DateOnly.FromDateTime(DateTime.Today));
            command.ExecuteNonQuery();
        }

        void Migrate()
        {
            bool didMigrate = false;
            foreach (string dbVersionString in DBVersionStringToMigrationScript.Keys)
            {
                if (string.Compare(AppInfo.DatabaseVersion, dbVersionString) < 0)
                {
                    didMigrate = true;
                    ReadAndRunSqlScript(DBVersionStringToMigrationScript[dbVersionString]);
                    AppInfo.DatabaseVersion = dbVersionString;
                }
            }

            if (didMigrate)
            {
                UpdateLastUpdated();
            }
        }

        void ReadAndRunSqlScript(string scriptFile)
        {
            string fullPath = Path.Join(MigrationScriptsPath, scriptFile);
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = File.ReadAllText(fullPath);
            command.ExecuteNonQuery();
        }
    }
}