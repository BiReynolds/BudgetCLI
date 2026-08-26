namespace BudgetCLI.Exceptions
{
    public class CannotGetTokenFromWordException : Exception
    {
        public CannotGetTokenFromWordException(string word) : base($"Scanner could not get token from word {word}")
        {

        }
    }
}