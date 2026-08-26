using BudgetCLI.Core.Objects;

namespace BudgetCLI.Exceptions
{
    public class OutputTokenNotSupportedException : Exception 
    {
        public OutputTokenNotSupportedException(OutputTokenBase outputToken) : 
        base($"This renderer does not support output tokens of type {outputToken.OutputTokenType}") { }
    }
}