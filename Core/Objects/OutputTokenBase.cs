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
        EXIT_NOTIFICATION,
        SAVE_NOTIFICATION,
        RESET_NOTIFICATION,
        SIMPLE_TEXT,
        ERROR_TEXT,
        SINGLE_ONE_TIME_BILL,
        ONE_TIME_BILL_LIST,
        ERROR,
        SINGLE_RECURRING_BILL,
        RECURRING_BILL_LIST,
        PROJECTION_TABLE,
        PROJECTION_SUMMARY
    }
}