using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Evaluator.Objects
{
    public class BooleanFilterInfo : FilterInfo
    {
        public bool FilterValue;
        public BooleanFilterInfo(ReservedWordToken fieldToken, bool filterValue) : base(fieldToken, new ReservedWordToken("=", ReservedWordEnum.EQUAL))
        {
            FilterValue = filterValue;
        }
    }
}