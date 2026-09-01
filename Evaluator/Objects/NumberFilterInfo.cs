using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Evaluator.Objects
{
    public class NumberFilterInfo : FilterInfo
    {
        public decimal FilterValue;
        public NumberFilterInfo(ReservedWordToken fieldToken, ReservedWordToken comparatorToken, NumberToken filterValueToken) : base(fieldToken, comparatorToken)
        {
            FilterValue = filterValueToken.Value;
        }
    }
}