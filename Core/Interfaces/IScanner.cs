using BudgetCLI.Core.Objects;

namespace BudgetCLI.Core.Interfaces
{
    public interface IScanner
    {
        public List<BudgetTokenBase> Scan(string rawString);
    }
}