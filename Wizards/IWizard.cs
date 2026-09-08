using BudgetCLI.Core.Objects;

namespace BudgetCLI.Wizards
{
    public interface IWizard
    {
        public List<List<BudgetTokenBase>> GetCommandsFromWizard();
    }
}