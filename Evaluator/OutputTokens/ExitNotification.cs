using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class ExitNotification : OutputTokenBase
    {
        public bool ExitSuccess;
        public readonly string SuccessfulExitText = "Exiting program";
        public readonly string UnsuccessfulExitText = "Cannot exit with unsaved changes - either reset or save before exiting";
        public ExitNotification(bool exitSuccess) : base(OutputTokenEnum.EXIT_NOTIFICATION)
        {
            ExitSuccess = exitSuccess;
        }
    }
}