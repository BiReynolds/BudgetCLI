using BudgetCLI.Core.Objects;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Wizards
{
    public class StartupWizard : IWizard
    {
        public StartupWizard() 
        {
           
        }

        public List<List<BudgetTokenBase>> GetCommandsFromWizard()
        {
            List<List<BudgetTokenBase>> result = new();
            WizardHelper.AddToListIfNotEmpty(result, GetUpdateBalanceCommand());
            WizardHelper.AddToListIfNotEmpty(result, GetProjectionSummaryCommand());
            WizardHelper.AddToListIfNotEmpty(result, GetShowBillsDueThisWeekCommand());
            return result;
        }

        List<BudgetTokenBase> GetUpdateBalanceCommand()
        {
            Console.Write("Session Balance: ");
            string? inputBalance = Console.ReadLine();
            NumberToken? balanceToken;
            if (inputBalance == null || inputBalance.IsWhiteSpace())
            {
                if (WizardHelper.ConfirmEmptyInput("Providing no input will set the Session Balance to 0.00.  This will make projections inaccurate."))
                {
                    balanceToken = NumberToken.Zero;
                }
                else
                {
                    return GetUpdateBalanceCommand();
                }
            }
            else if (!TokenHelper.TryGetNumberToken(inputBalance, out balanceToken))
            {
                Console.WriteLine($"Could not parse input {inputBalance} as a decimal number, please try again");
                return GetUpdateBalanceCommand();
            }
            return [new ReservedWordToken("edit", ReservedWordEnum.EDIT), new ReservedWordToken("balance", ReservedWordEnum.BALANCE), balanceToken ?? NumberToken.Zero];
        }

        List<BudgetTokenBase> GetProjectionSummaryCommand()
        {
            return [
                new ReservedWordToken("projection", ReservedWordEnum.PROJECTION),
                new ReservedWordToken("summary", ReservedWordEnum.SUMMARY)
            ];
        }

        List<BudgetTokenBase> GetShowBillsDueThisWeekCommand()
        {
            return [
                new ReservedWordToken("show", ReservedWordEnum.SHOW),
                new ReservedWordToken("bills", ReservedWordEnum.BILLS),
                new ReservedWordToken("duedate", ReservedWordEnum.DUE_DATE),
                new ReservedWordToken("<=", ReservedWordEnum.LESS_OR_EQUAL),
                DateToken.Today
            ];
        }

    }
}