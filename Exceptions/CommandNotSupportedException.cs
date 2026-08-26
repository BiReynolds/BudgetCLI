using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class CommandNotSupportedException : Exception
    {
        public CommandNotSupportedException(MainCommandToken commandToken) : 
        base($"This evaluator does not support tokens of type {commandToken.CommandType}") { } 
    }
}