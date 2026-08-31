using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class MainCommandToken : BudgetTokenBase
    {
        public BudgetMainCommandEnum CommandType;
        public MainCommandToken(string rawToken, BudgetMainCommandEnum commandType) : base(rawToken, BudgetTokenEnum.MAIN_COMMAND)
        {
            CommandType = commandType;
        }
    }

    public enum BudgetMainCommandEnum
    {
        EXIT,
        SHOW,
        ADD,
        DELETE,
        SAVE,
        RESET
    }

}