using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class SubCommandNotSupportedException : Exception
    {
        public SubCommandNotSupportedException(ReservedWordEnum mainCommandEnum, ReservedWordEnum ReservedWordEnum) : 
        base($"Command {mainCommandEnum} does not support subcommand {ReservedWordEnum}") {}
    }
}