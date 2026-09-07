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
            IEnumerable<OneTimeBillModel> deletedBills = SessionBillList.Where(x => x.IsDeleted);
            DatabaseHelper.DeleteManyOneTimeBills(deletedBills, Connection);
            IEnumerable<OneTimeBillModel> newBills = SessionBillList.Where(x => !x.IsDeleted && x.Id == -1);
            DatabaseHelper.AddManyOneTimeBillsToDatabase(newBills, Connection);
            IEnumerable<OneTimeBillModel> changedBills = SessionBillList.Where(x => !x.IsDeleted && x.Id != -1 && x.IsChanged);
            DatabaseHelper.UpdateManyOneTimeBills(changedBills, Connection);

            IEnumerable<RecurringBillModel> deletedRecurring = SessionRecurringBills.Where(x => x.IsDeleted);
            foreach (var recurringBillModel in deletedRecurring)
            {
                DatabaseHelper.DeleteRecurringBill(recurringBillModel, Connection);
            }
            IEnumerable<RecurringBillModel> newRecurring = SessionRecurringBills.Where(x => !x.IsDeleted && x.Id == null);
            foreach (var recurringBillModel in newRecurring)
            {
                DatabaseHelper.AddRecurringBillToDatabase(recurringBillModel, Connection);
            }
            IEnumerable<RecurringBillModel> changedRecurring = SessionRecurringBills.Where(x => !x.IsDeleted && x.Id != null && x.IsChanged);
            foreach (var recurringBillModel in changedRecurring)
            {
                DatabaseHelper.UpdateRecurringBill(recurringBillModel, Connection);
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