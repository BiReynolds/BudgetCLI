using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class NumberToken : BudgetTokenBase
    {
        public decimal Value { get; set; }
        public NumberToken(string rawToken, decimal value) : base(rawToken, BudgetTokenEnum.NUMBER)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}