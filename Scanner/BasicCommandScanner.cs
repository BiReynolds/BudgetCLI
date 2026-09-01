using BudgetCLI.Core.Interfaces;
using BudgetCLI.Core.Objects;
using BudgetCLI.Exceptions;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Scanner
{
    public class BasicCommandScanner : IScanner
    {
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
                string nextWord;
                if (char.IsWhiteSpace(currentChar))
                {
                    continue;
                }
                else if (currentChar == '\'')
                {
                    nextWord = ReadToNextSingleQuote();
                }
                else
                {
                    nextWord = ReadToNextWhiteSpace();
                }
                AddToken(nextWord);
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
            else if (TokenHelper.TryGetDateToken(word, out DateToken? dateResult))
            {
                result = dateResult;
            }
            else if (TokenHelper.TryGetReservedWord(word, out ReservedWordEnum? reservedWord) && reservedWord != null)
            {
                result = new ReservedWordToken(word, (ReservedWordEnum)reservedWord);
            }
            

            if (result == null)
            {
                throw new CannotGetTokenFromWordException(word);
            }
            else
            {
                OutputTokens.Add(result);
            }
        }

        public string ReadToNextWhiteSpace()
        {
            string result = RawString[CurrentIndex - 1].ToString(); // start of word was the last read character
            while (CurrentIndex < RawString.Length && !char.IsWhiteSpace(PeekChar()))
            {
                result += ReadChar();
            }
            return result;
        }

        public string ReadToNextSingleQuote()
        {
            string result = RawString[CurrentIndex - 1].ToString();
            while (CurrentIndex < RawString.Length && PeekChar() != '\'')
            {
                result += ReadChar();
            }
            // need to call one more time to capture the closing single quote
            result += ReadChar();
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
