using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Exceptions
{
    public class ExpectedSubCommandException : Exception
    {
        public ExpectedSubCommandException(BudgetMainCommandEnum mainCommandEnum) :
        base($"{mainCommandEnum} expects a subcommand") {}
    }
}