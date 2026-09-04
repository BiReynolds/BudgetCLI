using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using BudgetCLI.Evaluator.OutputTokens;
using Microsoft.Data.Sqlite;

namespace BudgetCLI.Session
{
    public class SessionManager
    {
        SqliteConnection Connection;
        public bool IsInitialized = false;
        public bool UnsavedChanges = false;
        public DateOnly Today;
        public decimal SessionBalance = 0;
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
                else if (recurringBillModel.IsDeleted)
                {
                    DatabaseHelper.DeleteRecurringBill(recurringBillModel, Connection);
                }
                else if (recurringBillModel.IsChanged)
                {
                    DatabaseHelper.UpdateRecurringBill(recurringBillModel, Connection);
                }
            }
            Connection.Close();
            ResetSession();
        }
        
        public OneTimeBillModel GetOneTimeBillByName(string name)
        {
            return SessionBillList.First(x => x.Name == name && !x.IsDeleted);
        }

        public RecurringBillModel GetRecurringBillModelByName(string name)
        {
            return SessionRecurringBills.First(x => x.Name == name && !x.IsDeleted);
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

        public void DeleteRecurringBillAndUnpaidInstances(RecurringBillModel recurringBill)
        {
            recurringBill.IsDeleted = true;
            IEnumerable<OneTimeBillModel> billInstances = SessionBillList.Where(x => (x.ParentId == recurringBill.Id) && !x.IsPaid);
            foreach (OneTimeBillModel model in billInstances)
            {
                model.IsDeleted = true;
            }
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