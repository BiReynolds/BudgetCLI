namespace BudgetCLI.Scanner.Tokens
{
    public static class TokenHelper
    {
        public static bool TryGetNumberToken(string tokenString, out NumberToken? result)
        {
            if (CheckNumberTokenFormat(tokenString))
            {
                float value = float.Parse(tokenString);
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
                if (currChar == '.' && i != tokenString.Length - 3)
                {
                    return false;
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
    }
}