namespace BudgetCLI.Data.Models
{
    public class RecurringBillModel
    {
        public event EventHandler? RecurringBillChanged;
        public int? Id { get; private set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsChanged
        {
            get;
            set
            {
                field = value;
                if (field)
                {
                    OnRecurringBillChanged(EventArgs.Empty);
                }
            }
        } = false;
        public string Name 
        { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    IsChanged = true;
                }
            }
        }
        public decimal Amount 
        { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    IsChanged = true;
                }
            }
        }
        public DateOnly StartDate 
        { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    IsChanged = true;
                }
            }
        }
        public DateOnly? EndDate 
        { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    IsChanged = true;
                }
            }
        }
        public RecurringTypeEnum RecurringType 
        { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    IsChanged = true;
                }
            }
        }
        public DateOnly ReferenceDate 
        { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    IsChanged = true;
                }
            }
        }

        public RecurringBillModel(int? id, string name, decimal amount, DateOnly startDate, RecurringTypeEnum recurringType)
        {
            Id = id;
            Name = name;
            Amount = amount;
            StartDate = startDate;
            EndDate = null;
            RecurringType = recurringType;
            ReferenceDate = startDate;
        }

        public RecurringBillModel(int? id, string name, decimal amount, DateOnly startDate, DateOnly? endDate, RecurringTypeEnum recurringType, DateOnly referenceDate)
        {
            Id = id;
            Name = name;
            Amount = amount;
            StartDate = startDate;
            EndDate = endDate;
            RecurringType = recurringType;
            ReferenceDate = referenceDate;
        }

        public void OnRecurringBillChanged(EventArgs e)
        {
            RecurringBillChanged?.Invoke(this, e);
        }
    }

    public enum RecurringTypeEnum
    {
        WEEKLY,
        BIWEEKLY,
        MONTHLY
    }
}