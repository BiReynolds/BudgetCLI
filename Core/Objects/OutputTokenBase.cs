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
        SIMPLE_TEXT
    }
}