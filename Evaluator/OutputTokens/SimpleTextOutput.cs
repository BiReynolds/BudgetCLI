using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class SimpleTextOutput : OutputTokenBase
    {
        public string Content;
        public SimpleTextOutput(List<BudgetTokenBase> tokensToShow) : base(OutputTokenEnum.SIMPLE_TEXT)
        {
            Content = string.Join(' ', tokensToShow);
        }

        public SimpleTextOutput(string content) : base(OutputTokenEnum.SIMPLE_TEXT)
        {
            Content = content;
        }
    }
}