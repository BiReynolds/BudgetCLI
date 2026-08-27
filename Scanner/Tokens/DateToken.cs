using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class DateToken : BudgetTokenBase
    {
        public DateOnly Value { get; set; }
        public DateToken(string rawToken, DateOnly value) : base(rawToken, BudgetTokenEnum.DATE)
        {
            Value = value;
        }
    }
}