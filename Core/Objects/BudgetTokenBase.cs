namespace BudgetCLI.Core.Objects
{
    public class BudgetTokenBase
    {
        public string RawToken;
        public BudgetTokenEnum TokenType;
        public BudgetTokenBase(string rawToken, BudgetTokenEnum tokenType)
        {
            RawToken = rawToken;
            TokenType = tokenType;
        }
    }
    
    public enum BudgetTokenEnum
    {
        RESERVED_WORD,
        EQUAL, 
        LESS_THAN,
        GREATER_THAN,
        LESS_OR_EQ,
        GREATER_OR_EQ,
        NUMBER,
        STRING,
        DATE,
    }
}