using BudgetCLI.Core.Objects;

namespace BudgetCLI.Core.Interfaces
{
    public interface IRenderer
    {
        public void Render(OutputTokenBase outputToken);
    }
}