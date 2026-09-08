using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class DateToken : BudgetTokenBase
    {
        public static readonly DateToken Today = new(DateTime.Today.ToString("yyyy/MM/dd"), DateOnly.FromDateTime(DateTime.Today));
        public DateOnly Value { get; set; }
        public DateToken(string rawToken, DateOnly value) : base(rawToken, BudgetTokenEnum.DATE)
        {
            Value = value;
        }
    }
}