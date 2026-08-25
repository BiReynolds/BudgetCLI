using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Scanner
{
    public class BasicCommandScanner
    {
        static Dictionary<string, BudgetMainCommandEnum> StringToCommandDict = new()
        {
            {"show", BudgetMainCommandEnum.SHOW}
        };
        string RawString = "";
        int CurrentIndex = 0;
        List<BudgetTokenBase> OutputTokens = [];
        public BasicCommandScanner()
        {
            
        }

        public List<BudgetTokenBase> Scan(string rawString)
        {
            RawString = rawString;
            CurrentIndex = 0;
            OutputTokens = [];
            char currentChar;
            while (CurrentIndex < rawString.Length)
            {
                currentChar = ReadChar();
                if (char.IsWhiteSpace(currentChar))
                {
                    continue;
                }
                else
                {
                    string nextWord = ReadToNextWhiteSpace();
                    AddToken(nextWord);
                }
            }
            return OutputTokens;
        }

        public void AddToken(string word)
        {
            BudgetTokenBase? result = null;
            if (TokenHelper.TryGetNumberToken(word, out NumberToken? numberResult))
            {
                result = numberResult;
            }
            else if (TokenHelper.TryGetStringToken(word, out StringToken? stringResult))
            {
                result = stringResult;
            }
            else if (StringToCommandDict.TryGetValue(word, out BudgetMainCommandEnum value))
            {
                result = new MainCommandToken(word, value);
            }
            if (result == null)
            {
                throw new Exception($"Could not get token from word {word}");
            }
            else
            {
                OutputTokens.Add(result);
            }
        }

        public string ReadToNextWhiteSpace()
        {
            string result = RawString[CurrentIndex - 1].ToString(); // start of word was the last read character
            while (!char.IsWhiteSpace(PeekChar()))
            {
                result += ReadChar();
            }
            return result;
        }

        public char ReadChar()
        {
            char result = RawString[CurrentIndex];
            CurrentIndex++;
            return result;
        }

        public char PeekChar()
        {
            return RawString[CurrentIndex];
        }
    }
}
