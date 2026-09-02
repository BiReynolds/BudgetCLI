namespace BudgetCLI.Data.Models
{
    public class OneTimeBillModel
    {
        public event EventHandler? OneTimeBillModelChanged;
        public int? ParentId { get; private set; }
        public bool IsDeleted { get; set; }
        public bool IsChanged { 
            get; 
            private set
            {
                field = value;
                if (field)
                {
                    OnOneTimeBillModelChanged(EventArgs.Empty);
                }
            }
        }
        public int Id { get; private set; } = -1;
        public string Name { 
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
        public decimal Amount { 
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
        public DateOnly DueDate { 
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
        public bool IsPaid { 
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
        public OneTimeBillModel(string name, decimal amount, DateOnly dueDate, bool isPaid)
        {
            Id = -1;
            Name = name;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
            ParentId = null;
        }

        public OneTimeBillModel(string name, decimal amount, DateOnly dueDate, bool isPaid, int parentId)
        {
            Id = -1;
            Name = name;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
            ParentId = parentId;
        }

        public OneTimeBillModel(int id, string name, decimal amount, DateOnly dueDate, bool isPaid, int? parentId)
        {
            Id = id;
            Name = name;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
            ParentId = parentId;
        }

        public override string ToString()
        {
            string result = $"Id: {Id}";
            result += $"\nParent Id: {ParentId}";
            result += $"\nName: {Name}";
            result += $"\nAmount: {Amount}";
            result += $"\nDueDate: {DueDate}";
            result += $"\nIsPaid: {IsPaid}";
            return result;
        }

        private void OnOneTimeBillModelChanged(EventArgs e)
        {
            OneTimeBillModelChanged?.Invoke(this, e);
        }
    }
}