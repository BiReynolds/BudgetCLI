using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class RecurringBillDetail : OutputTokenBase
    {
        public int? Id;
        public string Name;
        public decimal Amount;
        public DateOnly StartDate;
        public DateOnly? EndDate;
        public RecurringTypeEnum RecurringType;
        public RecurringBillDetail(RecurringBillModel model) : base(OutputTokenEnum.SINGLE_RECURRING_BILL)
        {
            Id = model.Id;
            Name = model.Name;
            Amount = model.Amount;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            RecurringType = model.RecurringType;
        }
    }
}