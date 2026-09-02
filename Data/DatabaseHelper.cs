using Microsoft.Data.Sqlite;
using BudgetCLI.Data.Models;

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
            AddParameterWithNullableValueToCommand(command, "$parentId", newBill.ParentId);
            command.ExecuteNonQuery();
        }

        public static void AddManyOneTimeBillsToDatabase(IEnumerable<OneTimeBillModel> newBills, SqliteConnection connection)
        {
            // Per Microsoft docs, best practice for bulk insertion is to use a transaction and reuse the same parametrized command rather than a new command for each row
            using (var transaction = connection.BeginTransaction())
            {
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    INSERT INTO OneTimeBills (Name, Amount, DueDate, IsPaid, ParentId)
                    VALUES ($name, $amount, $dueDate, $isPaid, $parentId)
                """;

                var nameParameter = CreateParameterAndAddToCommand("$name", command);
                var amountParameter = CreateParameterAndAddToCommand("$amount", command);
                var dueDateParameter = CreateParameterAndAddToCommand("$dueDate", command);
                var isPaidParameter = CreateParameterAndAddToCommand("$isPaid", command);
                var parentIdParameter = CreateParameterAndAddToCommand("$parentId", command);

                foreach (OneTimeBillModel bill in newBills)
                {
                    nameParameter.Value = bill.Name;
                    amountParameter.Value = bill.Amount;
                    dueDateParameter.Value = bill.DueDate;
                    isPaidParameter.Value = bill.IsPaid;
                    parentIdParameter.Value = bill.ParentId;
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
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
                INSERT INTO RecurringBills (Name, Amount, StartDate, EndDate, RecurringType, ReferenceDate, LastOneTimeDueDateAdded)
                VALUES ($name, $amount, $startDate, $endDate, $recurringType, $referenceDate, $lastOneTimeDueDateAdded);
            """;
            command.Parameters.AddWithValue("$name", model.Name);
            command.Parameters.AddWithValue("$amount", model.Amount);
            command.Parameters.AddWithValue("$startDate", model.StartDate);
            AddParameterWithNullableValueToCommand(command, "$endDate", model.EndDate);
            command.Parameters.AddWithValue("$recurringType", model.RecurringType);
            command.Parameters.AddWithValue("$referenceDate", model.ReferenceDate);
            AddParameterWithNullableValueToCommand(command, "$lastOneTimeDueDateAdded", model.LastOneTimeDueDateAdded);
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
                return new RecurringBillModel(
                    reader.GetInt16(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    GetNullableValueFromReader(reader, (r, i) => DateOnly.FromDateTime(r.GetDateTime(i)), 4),
                    (RecurringTypeEnum)reader.GetInt16(5),
                    DateOnly.FromDateTime(reader.GetDateTime(6)),
                    GetNullableValueFromReader(reader, (r, i) => DateOnly.FromDateTime(r.GetDateTime(i)), 7)
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
                result.Add(new RecurringBillModel(
                    reader.GetInt16(0),
                    reader.GetString(1),
                    reader.GetDecimal(2),
                    DateOnly.FromDateTime(reader.GetDateTime(3)),
                    GetNullableValueFromReader(reader, (r, i) => DateOnly.FromDateTime(r.GetDateTime(i)), 4),
                    (RecurringTypeEnum)reader.GetInt16(5),
                    DateOnly.FromDateTime(reader.GetDateTime(6)),
                    GetNullableValueFromReader(reader, (r, i) => DateOnly.FromDateTime(r.GetDateTime(i)), 7)
                ));
            }
            return result;
        }
        public static List<BudgetJobModel> GetAllJobsFromDatabase(SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                SELECT Name, LastRunDate FROM BudgetJobs;
            """;

            List<BudgetJobModel> result = new();
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new BudgetJobModel(
                    reader.GetString(0),
                    GetNullableValueFromReader(reader, (r, i) => DateOnly.FromDateTime(r.GetDateTime(i)), 1)
                ));
            }
            return result;
        }

        public static void MarkJobComplete(string jobName, SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                UPDATE BudgetJobs
                SET LastRunDate = $today
                WHERE Name = $jobName
            """;
            command.Parameters.AddWithValue("$today", DateOnly.FromDateTime(DateTime.Today));
            command.Parameters.AddWithValue("$jobName", jobName);
            command.ExecuteNonQuery();
        }

        static SqliteParameter CreateParameterAndAddToCommand(string parameterName, SqliteCommand command)
        {
            SqliteParameter result = command.CreateParameter();
            result.ParameterName = parameterName;
            command.Parameters.Add(result);
            return result;
        }

        static void AddParameterWithNullableValueToCommand(SqliteCommand command, string parameterName, object? nullableValue)
        {
            if (nullableValue == null)
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, nullableValue);
            }
        }

        static T? GetNullableValueFromReader<T>(SqliteDataReader reader, Func<SqliteDataReader, int, T> notNullSelector, int ordinal) where T : struct
        {
            if (reader.IsDBNull(ordinal)) 
            {
                return null;
            }
            else
            {
                return notNullSelector(reader, ordinal);
            }
        }
    }
}