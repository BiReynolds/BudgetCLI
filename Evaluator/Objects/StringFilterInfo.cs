using BudgetCLI.Core.Objects;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Evaluator.Objects
{
    public class StringFilterInfo : FilterInfo
    {
        public string FilterValue;
        public StringFilterInfo(ReservedWordToken fieldToken, ReservedWordToken comparatorToken, StringToken filterValueToken) : 
        base(fieldToken, comparatorToken)
        {
            FilterValue = filterValueToken.Value;
        }
    }
}