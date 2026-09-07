using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class ReservedWordToken : BudgetTokenBase
    {
        public ReservedWordEnum ReservedWord;
        public ReservedWordToken(string rawToken, ReservedWordEnum reservedWordEnum) : base(rawToken, BudgetTokenEnum.RESERVED_WORD)
        {
            ReservedWord = reservedWordEnum;
        }
    }

    public enum ReservedWordEnum
    {
        EXIT,
        SHOW,
        ADD,
        DELETE,
        PAID,
        UNPAID,
        EDIT,
        SAVE,
        RESET,
        BILL,
        BILLS,
        NAME,
        AMOUNT,
        DUE_DATE,
        EQUAL, 
        LESS_THAN,
        GREATER_THAN,
        LESS_OR_EQUAL,
        GREATER_OR_EQUAL,
        CONTAINS,
        RECURRING,
        WEEKLY,
        BIWEEKLY,
        FOUR_WEEKS,
        MONTHLY,
        PROJECTION,
        SUMMARY,
        BALANCE
    }
}