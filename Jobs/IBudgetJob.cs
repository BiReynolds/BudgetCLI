using BudgetCLI.Session;

namespace BudgetCLI.Jobs
{
    public interface IBudgetJob
    {
        public bool CheckDue(DateOnly lastRunDate);
        public void RunJob(SessionManager session);
    }
}