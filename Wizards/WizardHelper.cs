namespace BudgetCLI.Wizards
{
    public static class WizardHelper
    {
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

        public static void AddToListIfNotEmpty(List<string> stringList, string? newElement)
        {
            if (newElement == null || newElement.Length == 0)
            {
                return;
            }
            else
            {
                stringList.Add(newElement);
            }
        }

    }
}