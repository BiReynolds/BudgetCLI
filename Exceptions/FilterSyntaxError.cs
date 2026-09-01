using BudgetCLI.Core.Objects;

namespace BudgetCLI.Exceptions
{
    public class FilterSyntaxError : Exception
    {
        public FilterSyntaxError() : base("bad filter syntax") {}
    }
}