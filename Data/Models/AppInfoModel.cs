namespace BudgetCLI.Data.Models
{
    public class AppInfoModel
    {
        public bool IsInitialized = false;
        public string? AppVersion { get; set; }
        public string? DatabaseVersion { get; set; }
        public DateOnly? LastUpdated { get; set; }
        public DateOnly? LastOpened { get; set; }
        public AppInfoModel()
        {

        }
    }
}