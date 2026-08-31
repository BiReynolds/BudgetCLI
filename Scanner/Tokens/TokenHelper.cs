namespace BudgetCLI.Scanner.Tokens
{
    public static class TokenHelper
    {
        static Dictionary<string, BudgetMainCommandEnum> StringToCommandDict = new()
        {
            {"exit", BudgetMainCommandEnum.EXIT},
            {"show", BudgetMainCommandEnum.SHOW},
            {"add", BudgetMainCommandEnum.ADD},
            {"delete", BudgetMainCommandEnum.DELETE},
            {"paid", BudgetMainCommandEnum.PAID},
            {"unpaid", BudgetMainCommandEnum.UNPAID},
            {"save", BudgetMainCommandEnum.SAVE},
            {"reset", BudgetMainCommandEnum.RESET}
        };

        static Dictionary<string, SubCommandEnum> StringToSubCommandDict = new()
        {
            {"bill", SubCommandEnum.BILL},
            {"bills", SubCommandEnum.BILLS}
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

        public static bool TryGetCommandType(string tokenString, out BudgetMainCommandEnum? mainCommandToken)
        {
            BudgetMainCommandEnum result;
            if (StringToCommandDict.TryGetValue(tokenString, out result))
            {
                mainCommandToken = result;
                return true;
            }
            else
            {
                mainCommandToken = null;
                return false;
            }
        }

        public static bool TryGetSubCommand(string tokenString, out SubCommandEnum? subCommandEnum)
        {
            SubCommandEnum result;
            if (StringToSubCommandDict.TryGetValue(tokenString, out result))
            {
                subCommandEnum = result;
                return true;
            }
            else
            {
                subCommandEnum = null;
                return false;
            }
        }
    }
}