using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class SubCommandNotSupportedException : Exception
    {
        public SubCommandNotSupportedException(BudgetMainCommandEnum mainCommandEnum, SubCommandEnum subCommandEnum) : 
        base($"Command {mainCommandEnum} does not support subcommand {subCommandEnum}") {}
    }
}