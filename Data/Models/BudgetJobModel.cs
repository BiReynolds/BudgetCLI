namespace BudgetCLI.Data.Models
{
    public class BudgetJobModel
    {
        public string JobName;
        public DateOnly? LastRunDate;
        public BudgetJobModel(string jobName, DateOnly? lastRunDate)
        {
            JobName = jobName;
            LastRunDate = lastRunDate;
        }
    }
}