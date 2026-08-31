namespace BudgetCLI.Data.Models
{
    public class OneTimeBillModel
    {
        public event EventHandler? OneTimeBillModelChanged;
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
        public int Id { 
            get; 
            set
            {
                if (field != value)
                {
                    field = value;
                    IsChanged = true;
                }
            }
        } = -1;
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
        }
        public OneTimeBillModel(int id, string name, decimal amount, DateOnly dueDate, bool isPaid)
        {
            Id = id;
            Name = name;
            Amount = amount;
            DueDate = dueDate;
            IsPaid = isPaid;
        }

        public override string ToString()
        {
            string result = $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"Amount: {Amount}\n";
            result += $"DueDate: {DueDate}\n";
            result += $"IsPaid: {IsPaid}";
            return result;
        }

        private void OnOneTimeBillModelChanged(EventArgs e)
        {
            OneTimeBillModelChanged?.Invoke(this, e);
        }
    }
}