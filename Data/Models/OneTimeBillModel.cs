namespace BudgetCLI.Data.Models
{
    public class OneTimeBillModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateOnly DueDate { get; set; }
        public bool IsPaid { get; set; }
        public OneTimeBillModel(string name, DateOnly dueDate, bool isPaid)
        {
            Id = null;
            Name = name;
            DueDate = dueDate;
            IsPaid = isPaid;
        }
        public OneTimeBillModel(int id, string name, DateOnly dueDate, bool isPaid)
        {
            Id = id;
            Name = name;
            DueDate = dueDate;
            IsPaid = isPaid;
        }

        public override string ToString()
        {
            string result = $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"DueDate: {DueDate}\n";
            result += $"IsPaid: {IsPaid}";
            return result;
        }
    }
}