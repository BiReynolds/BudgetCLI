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

        public DateOnly? LastOneTimeDueDateAdded { get; set; }
        // When program creates new RecurringBillModel, it will call this constructor
        public RecurringBillModel(int? id, string name, decimal amount, DateOnly startDate, RecurringTypeEnum recurringType)
        {
            Id = id;
            Name = name;
            Amount = amount;
            StartDate = startDate;
            EndDate = null;
            RecurringType = recurringType;
            ReferenceDate = startDate;
            LastOneTimeDueDateAdded = null;
        }
        // When the db reads a RecurringBill from the db, it will call this constructor
        public RecurringBillModel(int? id, string name, decimal amount, DateOnly startDate, DateOnly? endDate, RecurringTypeEnum recurringType, DateOnly referenceDate, DateOnly? lastOneTimeDueDateAdded)
        {
            Id = id;
            Name = name;
            Amount = amount;
            StartDate = startDate;
            EndDate = endDate;
            RecurringType = recurringType;
            ReferenceDate = referenceDate;
            LastOneTimeDueDateAdded = lastOneTimeDueDateAdded;
        }

        public void OnRecurringBillChanged(EventArgs e)
        {
            RecurringBillChanged?.Invoke(this, e);
        }

        public OneTimeBillModel CreateOneTimeBillInstance(DateOnly dueDate)
        {
            if (Id == null)
            {
                throw new Exception("Cannot call CreateOneTimeBillInstance while Id is null - if this bill was just created, it should be written to the db and session should be reset before creating instances");
            }
            return new(Name, Amount, dueDate, false, (int)Id);
        }

        public List<OneTimeBillModel> GetNewBillInstances(DateOnly endDate)
        {
            switch (RecurringType)
            {
                case RecurringTypeEnum.WEEKLY:
                    return GetNewWeeklyBillInstances(endDate);
                case RecurringTypeEnum.BIWEEKLY:
                    return GetNewBiweeklyBillInstances(endDate);
                case RecurringTypeEnum.MONTHLY:
                    return GetNewMonthlyBillInstances(endDate);
                default:
                    throw new Exception($"RecurringType {RecurringType} is not supported");
            }
        }

        List<OneTimeBillModel> GetNewWeeklyBillInstances(DateOnly endDate)
        {
            DateOnly currDueDate = LastOneTimeDueDateAdded ?? StartDate;
            List<OneTimeBillModel> result = new();
            while (currDueDate < endDate)
            {
                result.Add(CreateOneTimeBillInstance(currDueDate));
                currDueDate = currDueDate.AddDays(7);
            }
            LastOneTimeDueDateAdded = result[^1].DueDate;
            return result;
        }
        List<OneTimeBillModel> GetNewBiweeklyBillInstances(DateOnly endDate)
        {
            DateOnly currDueDate = LastOneTimeDueDateAdded ?? StartDate;
            List<OneTimeBillModel> result = new();
            while (currDueDate < endDate)
            {
                result.Add(CreateOneTimeBillInstance(currDueDate));
                currDueDate = currDueDate.AddDays(14);
            }
            LastOneTimeDueDateAdded = result[^1].DueDate;
            return result;
        }
        List<OneTimeBillModel> GetNewMonthlyBillInstances(DateOnly endDate)
        {
            DateOnly currDueDate = LastOneTimeDueDateAdded ?? StartDate;
            List<OneTimeBillModel> result = new();
            while (currDueDate < endDate)
            {
                result.Add(CreateOneTimeBillInstance(currDueDate));
                currDueDate = currDueDate.AddMonths(1);
            }
            if (result.Count > 0)
            {
                LastOneTimeDueDateAdded = result[^1].DueDate;
            }
            return result;
        }

    }

    public enum RecurringTypeEnum
    {
        WEEKLY,
        BIWEEKLY,
        MONTHLY
    }
}