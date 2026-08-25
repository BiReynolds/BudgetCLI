using BudgetCLI.Scanner;

namespace BudgetCLI.Evaluator
{
    public abstract class OutputTokenBase
    {
        readonly OutputTokenEnum OutputTokenType;
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