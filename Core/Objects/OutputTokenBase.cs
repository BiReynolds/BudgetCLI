namespace BudgetCLI.Core.Objects
{
    public abstract class OutputTokenBase
    {
        public readonly OutputTokenEnum OutputTokenType;
        public OutputTokenBase(OutputTokenEnum outputTokenType)
        {
            OutputTokenType = outputTokenType;
        }
    }

    public enum OutputTokenEnum
    {
        SIMPLE_TEXT,
        ERROR_TEXT,
        SINGLE_ONE_TIME_BILL,
        ONE_TIME_BILL_LIST
    }
}