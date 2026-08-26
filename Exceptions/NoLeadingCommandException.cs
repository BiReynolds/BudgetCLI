using BudgetCLI.Core.Objects;

namespace BudgetCLI.Exceptions
{
    public class NoLeadingCommandException : Exception
    {
        public NoLeadingCommandException(BudgetTokenBase firstToken) : 
        base($"First token in input should be a recognized command, instead received {firstToken.RawToken} of type {firstToken.TokenType}") { }
    }
}