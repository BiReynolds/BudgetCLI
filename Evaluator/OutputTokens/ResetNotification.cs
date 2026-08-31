using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class ResetNotification : OutputTokenBase
    {
        public readonly string ResetText = "Reset session data";
        public ResetNotification() : base(OutputTokenEnum.RESET_NOTIFICATION)
        {
        }
    }
}