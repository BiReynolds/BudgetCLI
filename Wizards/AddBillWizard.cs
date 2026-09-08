using BudgetCLI.Core.Objects;
using BudgetCLI.Renderer;
using BudgetCLI.Scanner.Tokens;
using Microsoft.VisualBasic;

namespace BudgetCLI.Wizards
{
    public class AddBillWizard : IWizard
    {
        public AddBillWizard() {}
        public List<List<BudgetTokenBase>> GetCommandsFromWizard()
        {
            List<BudgetTokenBase> command = [new ReservedWordToken("add", ReservedWordEnum.ADD), new ReservedWordToken("bill", ReservedWordEnum.BILL)];

            bool shouldExit = false;
            StringToken nameStringToken = GetNameToken(out shouldExit);
            if (shouldExit)
            {
                return [];
            }
            else
            {
                command.Add(nameStringToken);
            }
            
            NumberToken amountToken = GetAmountToken(out shouldExit);
            if (shouldExit)
            {
                return [];
            }
            else
            {
                command.Add(amountToken);
            }

            DateToken dueDateToken = GetDueDateToken(out shouldExit);
            if (shouldExit)
            {
                return [];
            }
            else
            {
                command.Add(dueDateToken);
            }

            ReservedWordToken? recurringTypeToken = GetRecurringTypeToken(out shouldExit);
            if (shouldExit)
            {
                return [];
            }
            else if (recurringTypeToken == null)
            {
                return [command];
            }
            else
            {
                command.Add(recurringTypeToken);
                return [command];
            }
        }

        StringToken GetNameToken(out bool shouldExit)
        {
            shouldExit = false;
            RenderHelper.SetColorsToDefaultResponse();
            Console.Write("Name: ");
            RenderHelper.SetColorsToDefaultInput();
            string? inputName = Console.ReadLine();
            if (WizardHelper.IsExitString(inputName))
            {
                shouldExit = true;
                return StringToken.Empty;
            }
            else if (inputName == null || inputName.IsWhiteSpace())
            {
                Console.WriteLine($"Must provide a name (to cancel and exit this wizard, input {WizardHelper.ForcedExitString})");
                return GetNameToken(out shouldExit);
            }
            else
            {
                shouldExit = false;
                return new StringToken(inputName);
            }
        }

        NumberToken GetAmountToken(out bool shouldExit)
        {
            shouldExit = false;
            RenderHelper.SetColorsToDefaultResponse();
            Console.Write("Amount: ");
            RenderHelper.SetColorsToDefaultInput();
            string? inputAmount = Console.ReadLine();
            if (WizardHelper.IsExitString(inputAmount))
            {
                shouldExit = true;
                return NumberToken.Zero;
            }
            else if (inputAmount == null || inputAmount.IsWhiteSpace())
            {
                RenderHelper.SetColorsToDefaultResponse();
                Console.WriteLine($"Must provide an amount (to cancel and exit this wizard, input {WizardHelper.ForcedExitString})");
                return GetAmountToken(out shouldExit);
            }
            else if (TokenHelper.TryGetNumberToken(inputAmount, out NumberToken? result) && result != null)
            {
                return result;
            }
            else
            {
                RenderHelper.SetColorsToDefaultResponse();
                Console.WriteLine($"Could not parse input {inputAmount} as a decimal number, please try again");
                return GetAmountToken(out shouldExit);
            }
        }

        DateToken GetDueDateToken(out bool shouldExit)
        {
            shouldExit = false;
            RenderHelper.SetColorsToDefaultResponse();
            Console.Write("Due Date (if recurring, the first Due Date): ");
            RenderHelper.SetColorsToDefaultInput();
            string? inputDate = Console.ReadLine();
            if (WizardHelper.IsExitString(inputDate))
            {
                shouldExit = true;
                return DateToken.Today;
            }
            else if (inputDate == null || inputDate.IsWhiteSpace())
            {
                RenderHelper.SetColorsToDefaultResponse();
                Console.WriteLine($"Must provide a due date (to cancel and exit this wizard, input {WizardHelper.ForcedExitString})");
                return GetDueDateToken(out shouldExit);
            }
            else if (TokenHelper.TryGetDateToken(inputDate, out DateToken? result) && result != null)
            {
                return result;
            }
            else
            {
                RenderHelper.SetColorsToDefaultResponse();
                Console.WriteLine($"Could not parse input {inputDate} as a date in YYYY/MM/DD format, please try again");
                return GetDueDateToken(out shouldExit);
            }
        }

        ReservedWordToken? GetRecurringTypeToken(out bool shouldExit)
        {
            shouldExit = false;
            ReservedWordEnum? recurringTypeAsReservedWord = WizardHelper.PromptForRecurringType(out string? rawInput);
            if (WizardHelper.IsExitString(rawInput))
            {
                shouldExit = true;
                return ReservedWordToken.Invalid;
            }
            else if (rawInput == null || rawInput.IsWhiteSpace())
            {
                RenderHelper.SetColorsToDefaultResponse();
                Console.WriteLine($"Must provide a response (to cancel and exit this wizard, input {WizardHelper.ForcedExitString})");
                return GetRecurringTypeToken(out shouldExit);
            }
            else if (recurringTypeAsReservedWord == ReservedWordEnum.INVALID)
            {
                RenderHelper.SetColorsToDefaultResponse();
                Console.WriteLine($"Not a valid input, please try again");
                return GetRecurringTypeToken(out shouldExit);
            }
            else if (recurringTypeAsReservedWord == null)
            {
                return null;
            }
            else
            {
                return new ReservedWordToken((ReservedWordEnum)recurringTypeAsReservedWord);
            }
        }
    }
}