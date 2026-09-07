using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class SubCommandNotSupportedException : Exception
    {
        public SubCommandNotSupportedException(ReservedWordEnum mainCommandEnum, ReservedWordEnum badSubCommand) : 
        base($"Command {mainCommandEnum} does not support subcommand {badSubCommand}") {}

        public SubCommandNotSupportedException(ReservedWordEnum[] startingCommands, ReservedWordEnum badSubCommand) : 
        base($"Command combination {string.Join(' ', startingCommands)} does not support subcommand {badSubCommand}") {}
    }
}