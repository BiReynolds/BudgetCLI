using BudgetCLI.Core.Objects;
using BudgetCLI.Session;

namespace BudgetCLI.Core.Interfaces
{
    public interface IEvaluator
    {
        public event EventHandler? SafeExitEvent;
        public event EventHandler? UnsavedChangesExitEvent;
        public void SetSessionData(SessionManager session);
        public OutputTokenBase Evaluate(List<BudgetTokenBase> tokens);
    }
}