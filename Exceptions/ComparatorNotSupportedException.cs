using BudgetCLI.Core.Objects;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class ComparatorNotSupportedException : Exception {
        public ComparatorNotSupportedException(BudgetTokenEnum filterType, ReservedWordEnum badComparator) : 
        base($"{filterType} does not support comparator {badComparator}") {}
    }
}