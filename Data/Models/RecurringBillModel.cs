namespace BudgetCLI.Data.Models
{
    public class RecurringBillModel
    {
        public int? Id { get; private set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public RecurringTypeEnum RecurringType { get; set; }
        public RecurringBillModel(int? id, string name, decimal amount, DateOnly startDate, RecurringTypeEnum recurringType)
        {
            Id = id;
            Name = name;
            Amount = amount;
            StartDate = startDate;
            EndDate = null;
            RecurringType = recurringType;
        }

        public RecurringBillModel(int? id, string name, decimal amount, DateOnly startDate, DateOnly? endDate, RecurringTypeEnum recurringType)
        {
            Id = id;
            Name = name;
            Amount = amount;
            StartDate = startDate;
            EndDate = endDate;
            RecurringType = recurringType;
        }
    }

    public enum RecurringTypeEnum
    {
        WEEKLY,
        BIWEEKLY,
        MONTHLY
    }
}