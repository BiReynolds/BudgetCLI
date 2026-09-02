namespace BudgetCLI.Scanner.Tokens
{
    public static class TokenHelper
    {
        static Dictionary<string, ReservedWordEnum> StringToReservedWord = new()
        {
            {"exit", ReservedWordEnum.EXIT},
            {"show", ReservedWordEnum.SHOW},
            {"add", ReservedWordEnum.ADD},
            {"delete", ReservedWordEnum.DELETE},
            {"paid", ReservedWordEnum.PAID},
            {"unpaid", ReservedWordEnum.UNPAID},
            {"edit", ReservedWordEnum.EDIT},
            {"save", ReservedWordEnum.SAVE},
            {"reset", ReservedWordEnum.RESET},
            {"bill", ReservedWordEnum.BILL},
            {"bills", ReservedWordEnum.BILLS},
            {"name", ReservedWordEnum.NAME},
            {"amount", ReservedWordEnum.AMOUNT},
            {"duedate", ReservedWordEnum.DUE_DATE},
            {"<", ReservedWordEnum.LESS_THAN},
            {">", ReservedWordEnum.GREATER_THAN},
            {"=", ReservedWordEnum.EQUAL},
            {"<=", ReservedWordEnum.LESS_OR_EQUAL},
            {">=", ReservedWordEnum.GREATER_OR_EQUAL},
            {"contains", ReservedWordEnum.CONTAINS},
            {"recurring", ReservedWordEnum.RECURRING}
        };

        public static bool TryGetNumberToken(string tokenString, out NumberToken? result)
        {
            if (CheckNumberTokenFormat(tokenString))
            {
                decimal value = decimal.Parse(tokenString);
                result = new NumberToken(tokenString, value);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        static bool CheckNumberTokenFormat(string tokenString)
        {
            for (int i = 0; i < tokenString.Length; i++)
            {
                char currChar = tokenString[i];
                if (currChar == '.')
                {
                    if (i != tokenString.Length - 3)
                    {
                        return false;
                    }
                }
                else if (!char.IsNumber(currChar))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool TryGetStringToken(string tokenString, out StringToken? result)
        {
            if (CheckStringTokenFormat(tokenString))
            {
                result = new StringToken(tokenString, tokenString[1..^1]);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        static bool CheckStringTokenFormat(string tokenString)
        {
            if (tokenString[0] != '\'' || tokenString[^1] != '\'')
            {
                return false;
            }
            for (int i = 1; i < tokenString.Length - 1; i++)
            {
                char currChar = tokenString[i];
                if (currChar == '\'' || currChar == '\"')
                {
                    return false;
                }
            }
            return true;
        }

        public static bool TryGetDateToken(string tokenString, out DateToken? result)
        {
            if (CheckDateFormat(tokenString))
            {
                int year = int.Parse(tokenString[0..4]);
                int month = int.Parse(tokenString[5..7]);
                int day = int.Parse(tokenString[8..10]);
                DateOnly date = new(year, month, day);
                result = new DateToken(tokenString, date);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        static bool CheckDateFormat(string tokenString)
        {
            for (int i = 0; i < tokenString.Length; i++)
            {
                if (i == 4 || i == 7)
                {
                    if (tokenString[i] != '/')
                    {
                        return false;
                    }
                }
                else if (!char.IsNumber(tokenString[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool TryGetReservedWord(string tokenString, out ReservedWordEnum? ReservedWordEnum)
        {
            if (StringToReservedWord.TryGetValue(tokenString.ToLower(), out ReservedWordEnum result))
            {
                ReservedWordEnum = result;
                return true;
            }
            else
            {
                ReservedWordEnum = null;
                return false;
            }
        }
    }
}