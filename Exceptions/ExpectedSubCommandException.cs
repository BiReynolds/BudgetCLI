using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class ExpectedSubCommandException : Exception
    {
        public ExpectedSubCommandException(ReservedWordEnum mainCommandEnum) :
        base($"{mainCommandEnum} expects a subcommand") {}
    }
}