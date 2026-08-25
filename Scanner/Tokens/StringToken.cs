namespace BudgetCLI.Scanner.Tokens
{
    public class StringToken : BudgetTokenBase
    {
        public string Value { get; set; }
        public StringToken(string rawToken, string value) : base(rawToken, BudgetTokenEnum.STRING)
        {
            Value = value;
        }
    }
}