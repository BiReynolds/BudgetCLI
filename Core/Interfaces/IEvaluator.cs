using BudgetCLI.Core.Objects;

namespace BudgetCLI.Core.Interfaces
{
    public interface IEvaluator
    {
        public OutputTokenBase Evaluate(List<BudgetTokenBase> tokens);
    }
}