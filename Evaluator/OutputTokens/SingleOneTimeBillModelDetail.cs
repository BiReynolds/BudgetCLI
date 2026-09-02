using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class SingleOneTimeBillModelDetail : OutputTokenBase
    {
        public int Id;
        public int? ParentId;
        public string Name;
        public decimal Amount;
        public DateOnly DueDate;
        public bool IsPaid;
        public SingleOneTimeBillModelDetail(OneTimeBillModel model) : base(OutputTokenEnum.SINGLE_ONE_TIME_BILL)
        {
            Id = model.Id;
            ParentId = model.ParentId;
            Name = model.Name;
            Amount = model.Amount;
            DueDate = model.DueDate;
            IsPaid = model.IsPaid;
        }
    }
}