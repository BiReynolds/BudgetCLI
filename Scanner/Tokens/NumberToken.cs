namespace BudgetCLI.Scanner.Tokens
{
    public class NumberToken : BudgetTokenBase
    {
        public float Value { get; set; }
        public NumberToken(string rawToken, float value) : base(rawToken, BudgetTokenEnum.NUMBER)
        {
            Value = value;
        }
    }
}