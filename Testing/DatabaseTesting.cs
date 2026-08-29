using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using Microsoft.Data.Sqlite;

namespace BudgetCLI.Testing
{
    public static class DatabaseTesting
    {
        public static void ResetTestData()
        {
            SqliteConnection connection = DatabaseHelper.GetReadWriteConnection();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = File.ReadAllText("./Testing/TestScripts/ResetTestOneTimeBills.sql");
            command.ExecuteNonQuery();
        }
        public static void OneTimeBillTest()
        {
            OneTimeBillModel testBill = new("OneTimeBillTest testBill", 100.00m, DateOnly.FromDateTime(DateTime.Today), false);
            SqliteConnection connection = DatabaseHelper.GetReadWriteConnection();
            connection.Open();
            SqliteTransaction transaction = connection.BeginTransaction();
            DatabaseHelper.AddOneTimeBillToDatabase(testBill, connection);
            Console.WriteLine("Added new bill to database: ");
            Console.WriteLine(testBill);
            OneTimeBillModel? billPulledByName = DatabaseHelper.GetOneTimeBillByName(testBill.Name, connection);
            if (billPulledByName == null)
            {
                transaction.Rollback();
                throw new Exception("OneTimeBillTest failed: Inserted testBill but could not look it up by name afterwards");
            }
            Console.WriteLine("Successfully pulled testBill from database by name");
            if (billPulledByName.Id == null)
            {
                transaction.Rollback();
                throw new Exception("OneTimeBillTest failed: billPulledByName has null Id");
            }
            else
            {
                OneTimeBillModel? billPulledById = DatabaseHelper.GetOneTimeBillById(billPulledByName.Id ?? 0, connection);
                if (billPulledById == null)
                {
                    transaction.Rollback();
                    throw new Exception("OneTimeBillTest failed: could not pull testBill from database by Id");
                }
                else
                {
                    Console.WriteLine("Successfully pulled testBill from database by Id");
                }

                if (DatabaseHelper.DeleteOneTimeBillById(billPulledByName.Id ?? 0, connection))
                {
                    Console.WriteLine("successfully deleted testBill");
                }
                else
                {
                    transaction.Rollback();
                    throw new Exception("OneTimeBilltest failed: failed to delete the bill which was added");
                }
                transaction.Rollback();
            }
        }
    }
}