using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class ErrorTextOutput : OutputTokenBase
    {
        public string ErrorInfo;

        public ErrorTextOutput(string content) : base(OutputTokenEnum.ERROR_TEXT)
        {
            ErrorInfo = content;
        }
    }
}