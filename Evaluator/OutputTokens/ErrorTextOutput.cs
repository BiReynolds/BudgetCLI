using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class ErrorTextOutput : OutputTokenBase
    {
        public string ErrorInfo;
        public ErrorTextOutput(List<BudgetTokenBase> tokensToShow) : base(OutputTokenEnum.SIMPLE_TEXT)
        {
            ErrorInfo = string.Join(' ', tokensToShow);
        }

        public ErrorTextOutput(string content) : base(OutputTokenEnum.SIMPLE_TEXT)
        {
            ErrorInfo = content;
        }
    }
}