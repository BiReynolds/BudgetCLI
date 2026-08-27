using BudgetCLI.Data.Models;
using Microsoft.Data.Sqlite;

namespace BudgetCLI.Data
{
    public class MigrationManager
    {
        static string MigrationScriptsPath = "./Data/MigrationScripts/";
        SqliteConnection Connection;
        AppInfoModel AppInfo = new();
        public MigrationManager()
        {
            Console.WriteLine("Initializing MigrationManager");
            Connection = DatabaseHelper.GetReadWriteConnection();
        }

        public void DoMigrations()
        {
            DatabaseHelper.EnsureDatabaseExists();
            if (!IsDatabaseInitialized())
            {
                InitializeDatabase();
            }
        }

        public bool IsDatabaseInitialized()
        {
            Connection.Open();
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
            Connection.Close();
            return result;
        }

        void InitializeDatabase()
        {
            Connection.Open();
            SqliteCommand command = Connection.CreateCommand();
            command.CommandText = File.ReadAllText(Path.Join(MigrationScriptsPath, "BudgetDatabaseCreation.sql"));
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}