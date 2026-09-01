using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Evaluator.Objects
{
    public class DateFilterInfo : FilterInfo
    {
        public DateOnly FilterValue;
        public DateFilterInfo(ReservedWordToken fieldToken, ReservedWordToken comparatorToken, DateToken filterValueToken) : base(fieldToken, comparatorToken)
        {
            FilterValue = filterValueToken.Value;
        }
    }
}