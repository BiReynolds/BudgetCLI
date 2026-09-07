namespace BudgetCLI.Wizards
{
    public class StartupWizard : IWizard
    {
        public StartupWizard() 
        {
           
        }

        public List<string> GetCommandsFromWizard()
        {
            List<string> result = new();
            WizardHelper.AddToListIfNotEmpty(result, GetUpdateBalanceCommand());
            WizardHelper.AddToListIfNotEmpty(result, "projection summary");
            WizardHelper.AddToListIfNotEmpty(result, GetShowBillsDueThisWeekCommand());
            return result;
        }

        string GetUpdateBalanceCommand()
        {
            Console.Write("Session Balance: ");
            string? inputBalance = Console.ReadLine();
            if (inputBalance == null || inputBalance.IsWhiteSpace())
            {
                if (WizardHelper.ConfirmEmptyInput("Providing no input will set the Session Balance to 0.00.  This will make projections inaccurate."))
                {
                    return "";
                }
                else
                {
                    return GetUpdateBalanceCommand();
                }
            }
            else
            {
                return $"edit balance {inputBalance}";
            }

        }

        string GetShowBillsDueThisWeekCommand()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            return $"show bills duedate <= {today.AddDays(7).ToString("yyyy/MM/dd")}";
        }

    }
}