namespace BudgetCLI.Core.Objects
{
    public class ErrorToken : OutputTokenBase
    {
        public Exception EncounteredException;
        public ErrorToken(Exception exception) : base(OutputTokenEnum.ERROR)
        {
            EncounteredException = exception;
        }
    }
}