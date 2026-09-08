using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;
using BudgetCLI.Renderer;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Wizards
{
    public static class WizardHelper
    {
        public static Dictionary<RecurringTypeEnum, ReservedWordEnum?> RecurringTypeToReservedWordDict = new()
        {
            {RecurringTypeEnum.NOT_RECURRING, null},
            {RecurringTypeEnum.WEEKLY, ReservedWordEnum.WEEKLY},
            {RecurringTypeEnum.BIWEEKLY, ReservedWordEnum.BIWEEKLY},
            {RecurringTypeEnum.MONTHLY, ReservedWordEnum.MONTHLY},
            {RecurringTypeEnum.FOUR_WEEKS, ReservedWordEnum.FOUR_WEEKS}
        };
        public static string ForcedExitString = "!cancel";
        public static bool ConfirmEmptyInput(string helperText)
        {
            Console.WriteLine(helperText);
            Console.WriteLine("Press Enter again to confirm, or input any string to go back.");
            string? result = Console.ReadLine();
            if (result == null || result.IsWhiteSpace())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void AddToListIfNotEmpty(List<List<BudgetTokenBase>> stringList, List<BudgetTokenBase> newCommand)
        {
            if (newCommand == null || newCommand.Count == 0)
            {
                return;
            }
            else
            {
                stringList.Add(newCommand);
            }
        }

        public static bool IsExitString(string? userInput)
        {
            if (userInput != null && userInput == ForcedExitString)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ReservedWordEnum? PromptForRecurringType(out string? rawInput)
        {
            RenderHelper.SetColorsToDefaultResponse();
            Console.WriteLine("Choose from the following recurring types by inputting the corresponding number: ");
            foreach (RecurringTypeEnum recurringType in Enum.GetValues<RecurringTypeEnum>())
            {
                Console.WriteLine($"{(int)recurringType} : {RenderHelper.RecurringTypeToRenderableString[recurringType]}");
            }
            Console.Write("Recurring Type: ");
            RenderHelper.SetColorsToDefaultInput();
            rawInput = Console.ReadLine();
            if (int.TryParse(rawInput, out int selection))
            {
                return RecurringTypeToReservedWordDict[(RecurringTypeEnum)selection];
            }
            else
            {
                return ReservedWordEnum.INVALID;
            }
        }
    }
}