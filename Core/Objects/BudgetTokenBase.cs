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
        NUMBER,
        STRING,
        DATE,
        MAIN_COMMAND,
        SUB_COMMAND,
        ERROR
    }
}