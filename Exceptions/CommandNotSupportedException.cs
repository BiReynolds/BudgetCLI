using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class CommandNotSupportedException : Exception
    {
        public CommandNotSupportedException(ReservedWordToken commandToken) : 
        base($"This evaluator does not support tokens of type {commandToken.ReservedWord}") { } 
    }
}