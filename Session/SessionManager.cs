using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using Microsoft.Data.Sqlite;

namespace BudgetCLI.Session
{
    public class SessionManager
    {
        SqliteConnection Connection;
        public bool IsInitialized = false;
        public bool UnsavedChanges = false;
        public DateOnly Today;
        public List<OneTimeBillModel> SessionBillList = [];
        public List<RecurringBillModel> SessionRecurringBills = [];
        public SessionManager()
        {
            Connection = DatabaseHelper.GetReadWriteConnection();
            Today = DateOnly.FromDateTime(DateTime.Today);
        }

        public void InitSession()
        {
            Connection.Open();
            SessionBillList = DatabaseHelper.GetAllOneTimeBills(Connection);
            SessionRecurringBills = DatabaseHelper.GetAllRecurringBills(Connection);
            foreach (OneTimeBillModel model in SessionBillList)
            {
                RegisterEventsForOneTimeBill(model);
            }

            Connection.Close();
            IsInitialized = true;
        }

        public void ResetSession()
        {
            UnsavedChanges = false;
            InitSession();
        }

        public void SaveSession()
        {
            Connection.Open();
            foreach (OneTimeBillModel billModel in SessionBillList)
            {
                if (billModel.Id == -1) {
                    if (!billModel.IsDeleted)
                    {
                        DatabaseHelper.AddOneTimeBillToDatabase(billModel, Connection);
                    }
                }
                else if (billModel.IsDeleted)
                {
                    DatabaseHelper.DeleteOneTimeBillById(billModel.Id, Connection);
                }
                else if (billModel.IsChanged)
                {
                    DatabaseHelper.UpdateOneTimeBill(billModel, Connection);
                }
            }
            foreach (RecurringBillModel recurringBillModel in SessionRecurringBills)
            {
                if (recurringBillModel.Id == null)
                {
                    if (!recurringBillModel.IsDeleted)
                    {
                        DatabaseHelper.AddRecurringBillToDatabase(recurringBillModel, Connection);
                    }
                }
                // TODO: Handle recurring bill deletion
                // TODO: Handle recurring bill editing
            }
            Connection.Close();
            ResetSession();
        }
        
        public OneTimeBillModel GetOneTimeBillByName(string name)
        {
            return SessionBillList.First(x => x.Name == name);
        }

        public RecurringBillModel GetRecurringBillModelByName(string name)
        {
            return SessionRecurringBills.First(x => x.Name == name);
        }

        public void AddNewOneTimeBill(OneTimeBillModel billModel)
        {
            UnsavedChanges = true;
            SessionBillList.Add(billModel);
        }

        public void AddManyOneTimeBills(IEnumerable<OneTimeBillModel> billModels)
        {
            UnsavedChanges = true;
            SessionBillList.AddRange(billModels);
        }

        public void AddNewRecurringBill(RecurringBillModel recurringBillModel)
        {
            UnsavedChanges = true;
            SessionRecurringBills.Add(recurringBillModel);
        }

        public void DeleteOneTimeBill(OneTimeBillModel chosenBill)
        {
            chosenBill.IsDeleted = true;
            UnsavedChanges = true;
        }

        public void RegisterEventsForOneTimeBill(OneTimeBillModel model)
        {
            model.OneTimeBillModelChanged += (o, e) =>
            {
                UnsavedChanges = true;
            };
        }
    }
}