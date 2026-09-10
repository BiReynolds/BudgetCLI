using BudgetCLI.Core.Objects;

namespace BudgetCLI.Scanner.Tokens
{
    public class ReservedWordToken : BudgetTokenBase
    {
        public static readonly ReservedWordToken Invalid = new("", ReservedWordEnum.INVALID);
        public ReservedWordEnum ReservedWord;
        public ReservedWordToken(string rawToken, ReservedWordEnum reservedWordEnum) : base(rawToken, BudgetTokenEnum.RESERVED_WORD)
        {
            ReservedWord = reservedWordEnum;
        }

        public ReservedWordToken(ReservedWordEnum reservedWordEnum) : base(reservedWordEnum.ToString(), BudgetTokenEnum.RESERVED_WORD)
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
        BALANCE,
        NEXT_DUE,
        END_DATE,
        INVALID,
        ALLOWANCE
    }
}