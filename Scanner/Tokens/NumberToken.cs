using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class NumberToken : BudgetTokenBase
    {
        public float Value { get; set; }
        public NumberToken(string rawToken, float value) : base(rawToken, BudgetTokenEnum.NUMBER)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}