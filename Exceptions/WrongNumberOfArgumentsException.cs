namespace BudgetCLI.Exceptions
{
    public class WrongNumberOfArgumentsException : Exception
    {
        public WrongNumberOfArgumentsException(int wrongNumArguments, int expectedNumArguments) :
        base($"Expected {expectedNumArguments} arguments, received {wrongNumArguments}") {}

        public WrongNumberOfArgumentsException(int wrongNumArguments, int[] expectedNumArguments) :
        base($"Expected {string.Join(" | ", expectedNumArguments)} arguments, received {wrongNumArguments}") {}
    }
}