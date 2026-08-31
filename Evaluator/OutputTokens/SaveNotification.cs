using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class SaveNotification : OutputTokenBase
    {
        public readonly string SaveText = "Saved session data";
        public SaveNotification() : base(OutputTokenEnum.SAVE_NOTIFICATION)
        {
        }
    }
}