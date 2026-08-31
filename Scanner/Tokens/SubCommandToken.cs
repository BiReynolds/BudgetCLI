using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class SubCommandToken : BudgetTokenBase
    {
        public SubCommandEnum SubCommandType;
        public SubCommandToken(string rawToken, SubCommandEnum subCommandType) : base(rawToken, BudgetTokenEnum.SUB_COMMAND)
        {
            SubCommandType = subCommandType;
        }
    }

    public enum SubCommandEnum
    {
        BILL,
        BILLS,
        NAME,
        AMOUNT,
        DUE_DATE
    }
}