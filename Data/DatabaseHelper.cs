using Microsoft.Data.Sqlite;
using BudgetCLI.Data.Models;
using System.Data.Common;

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
                FileStream file = File.Create(DatabasePath);
                file.Close();
            }
        }

        public static SqliteConnection GetReadWriteConnection()
        {
            return new SqliteConnection($"Data Source={DatabasePath};Mode=ReadWrite"); 
        }

        public static void AddOneTimeBillToDatabase(OneTimeBillModel newBill, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                INSERT INTO OneTimeBills (Name, Amount, DueDate, IsPaid, ParentId)
                VALUES ($name, $amount, $dueDate, $isPaid, $parentId);
            """;
            command.Parameters.AddWithValue("$name", newBill.Name);
            command.Parameters.AddWithValue("$amount", newBill.Amount);
            command.Parameters.AddWithValue("$dueDate", newBill.DueDate);
            command.Parameters.AddWithValue("$isPaid", newBill.IsPaid);
            if (newBill.ParentId == null)
            {
                command.Parameters.AddWithValue("$parentId", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("$parentId", newBill.ParentId);
            }
            command.ExecuteNonQuery();
        }

        public static OneTimeBillModel? GetOneTimeBillById(int id, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                SELECT * FROM OneTimeBills
                WHERE Id = $id;
            """;
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new OneTimeBillModel(
                    reader.GetInt16(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    reader.GetBoolean(4),
                    reader.GetInt16(5)
                );
            }
            else
            {
                return null;
            }
        }

        public static OneTimeBillModel? GetOneTimeBillByName(string name, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                SELECT * FROM OneTimeBills
                WHERE Name = $name;
            """;
            command.Parameters.AddWithValue("$name", name);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new OneTimeBillModel(
                    reader.GetInt16(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    reader.GetBoolean(4),
                    reader.GetInt16(5)
                );
            }
            else
            {
                return null;
            }
        }

        public static List<OneTimeBillModel> GetAllOneTimeBills(SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                SELECT * FROM OneTimeBills;
            """;
            SqliteDataReader reader = command.ExecuteReader();
            List<OneTimeBillModel> result = new();
            while (reader.Read())
            {
                int? parentId;
                if (reader.IsDBNull(5))
                {
                    parentId = null;
                }
                else
                {
                    parentId = reader.GetInt16(5);
                }
                result.Add(new OneTimeBillModel(
                    reader.GetInt16(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    reader.GetBoolean(4),
                    parentId
                ));
            }
            return result;
        }

        public static bool DeleteOneTimeBillById(int id, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                DELETE FROM OneTimeBills
                WHERE Id = $id;
            """;
            command.Parameters.AddWithValue("$id", id);
            int numDeletions = command.ExecuteNonQuery();
            // return value indicates whether a value was actually deleted from the db - i.e. if there was actually a value in the db with that id
            return numDeletions > 0;
        }

        public static void UpdateOneTimeBill(OneTimeBillModel updatedModel, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                UPDATE OneTimeBills
                SET 
                Name = $name,
                Amount = $amount,
                DueDate = $dueDate,
                IsPaid = $isPaid
                WHERE Id = $id
            """;
            command.Parameters.AddWithValue("$name", updatedModel.Name);
            command.Parameters.AddWithValue("$amount", updatedModel.Amount);
            command.Parameters.AddWithValue("$dueDate", updatedModel.DueDate);
            command.Parameters.AddWithValue("$isPaid", updatedModel.IsPaid);
            command.Parameters.AddWithValue("$id", updatedModel.Id);
            command.ExecuteNonQuery();
        }

        public static void AddRecurringBillToDatabase(RecurringBillModel model, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                INSERT INTO RecurringBills (Name, Amount, StartDate, EndDate, RecurringType, ReferenceDate)
                VALUES ($name, $amount, $startDate, $endDate, $recurringType, $referenceDate);
            """;
            command.Parameters.AddWithValue("$name", model.Name);
            command.Parameters.AddWithValue("$amount", model.Amount);
            command.Parameters.AddWithValue("$startDate", model.StartDate);
            command.Parameters.AddWithValue("$endDate", model.EndDate);
            command.Parameters.AddWithValue("$recurringType", model.RecurringType);
            command.Parameters.AddWithValue("$referenceDate", model.ReferenceDate);
            if (model.EndDate == null)
            {
                command.Parameters.AddWithValue("$endDate", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("$endDate", model.EndDate);
            }
            command.ExecuteNonQuery();
        }

        public static RecurringBillModel? GetRecurringBillModelByName(string name, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                SELECT * FROM RecurringBills
                WHERE Name = $name;
            """;
            command.Parameters.AddWithValue("$name", name);

            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                DateOnly? endDate;
                if (reader.IsDBNull(4))
                {
                    endDate = null;
                }
                else
                {
                    endDate = DateOnly.FromDateTime(reader.GetDateTime(4));
                }
                return new RecurringBillModel(
                    reader.GetInt16(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    endDate,
                    (RecurringTypeEnum)reader.GetInt16(5),
                    DateOnly.FromDateTime(reader.GetDateTime(6))
                );
            }
            else
            {
                return null;
            }
        }

        public static List<RecurringBillModel> GetAllRecurringBills(SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                SELECT * FROM RecurringBills
            """;

            SqliteDataReader reader = command.ExecuteReader();
            List<RecurringBillModel> result = new();
            while (reader.Read())
            {
                DateOnly? endDate;
                if (reader.IsDBNull(4))
                {
                    endDate = null;
                }
                else
                {
                    endDate = DateOnly.FromDateTime(reader.GetDateTime(4));
                }
                result.Add(new RecurringBillModel(
                    reader.GetInt16(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    endDate,
                    (RecurringTypeEnum)reader.GetInt16(5),
                    DateOnly.FromDateTime(reader.GetDateTime(6))
                ));
            }
            return result;
        }
    }
}