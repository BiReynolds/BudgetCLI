using Microsoft.Data.Sqlite;

namespace BudgetCLI.Data
{
    public static class DatabaseHelper
    {
        static string DatabasePath = "./db/Budget.db";

        public static void EnsureDatabaseExists()
        {
            if (!File.Exists(DatabasePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath) ?? "");
                File.Create(DatabasePath);
            }
        }

        public static SqliteConnection GetReadWriteConnection()
        {
            return new SqliteConnection($"Data Source={DatabasePath};Mode=ReadWrite"); 
        }

    }
}