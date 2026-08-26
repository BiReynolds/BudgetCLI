using BudgetCLI.Core.Objects;

namespace BudgetCLI.Exceptions
{
    public class TokenTypeMismatchException : Exception
    {
        public TokenTypeMismatchException(BudgetTokenEnum tokenTypeAsEnum, Type runtimeTokenType) : 
        base($"Token has TokenType value of {tokenTypeAsEnum}, but could not be interpreted as type {runtimeTokenType}") { }
    }
}