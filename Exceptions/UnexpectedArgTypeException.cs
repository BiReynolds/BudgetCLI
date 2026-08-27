using BudgetCLI.Core.Objects;

namespace BudgetCLI.Exceptions
{
    public class UnexpectedArgTypeException : Exception
    {
        public UnexpectedArgTypeException(BudgetTokenBase badArg, BudgetTokenEnum expectedType) :
        base($"{badArg} is of type {badArg.TokenType}, but command expected arg of type {expectedType}") {}
    }
}