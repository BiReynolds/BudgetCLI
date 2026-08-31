using BudgetCLI.Core.Objects;

namespace BudgetCLI.Exceptions
{
    public class UnexpectedArgTypeException : Exception
    {
        public UnexpectedArgTypeException(BudgetTokenBase badArg, BudgetTokenEnum expectedType) :
        base($"{badArg.RawToken} is of type {badArg.TokenType}, but command expected arg of type {expectedType}") {}

        public UnexpectedArgTypeException(BudgetTokenBase badArg, IEnumerable<BudgetTokenEnum> expectedTypes) : 
        base($"{badArg.RawToken} is of type {badArg.TokenType}, but command expected arg of one of the following types: \n{string.Join(" | ", expectedTypes)}") {}
    }
}