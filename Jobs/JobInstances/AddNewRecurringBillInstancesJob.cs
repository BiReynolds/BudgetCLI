using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using BudgetCLI.Session;

namespace BudgetCLI.Jobs.JobInstances
{
    public class AddNewRecurringBillInstancesJob : IBudgetJob
    {
        public static int NumMonthsLookahead = 12;
        public AddNewRecurringBillInstancesJob() { }

        public bool CheckDue(DateOnly lastRunDate)
        {
            return true;
        }

        public void RunJob(SessionManager session)
        {
            DateOnly endDate = DateOnly.FromDateTime(DateTime.Today).AddMonths(NumMonthsLookahead);
            foreach (RecurringBillModel recurringBill in session.SessionRecurringBills)
            {
                List<OneTimeBillModel> newInstances = recurringBill.GetNewBillInstances(endDate);
                session.AddManyOneTimeBills(newInstances);
            }
            session.SaveSession();
            session.ResetSession();
        }
    }
}