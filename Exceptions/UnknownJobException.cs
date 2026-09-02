namespace BudgetCLI.Exceptions
{
    public class UnknownJobException : Exception
    {
        public UnknownJobException(string jobName) : 
        base($"JobManager does not know of a job named {jobName}") {}
    }
}