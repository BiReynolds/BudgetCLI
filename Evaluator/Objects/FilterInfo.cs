using BudgetCLI.Core.Objects;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Evaluator.Objects
{
    public abstract class FilterInfo
    {
        public ReservedWordToken FieldToken;
        public ReservedWordToken ComparatorToken;
        public FilterInfo(ReservedWordToken fieldToken, ReservedWordToken comparatorToken)
        {
            FieldToken = fieldToken;
            ComparatorToken = comparatorToken;
        }
    }
}