using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class RecurringBillList : OutputTokenBase
    {
        public List<RecurringBillDetail> Data;
        public RecurringBillList(IEnumerable<RecurringBillModel> models) : base(OutputTokenEnum.RECURRING_BILL_LIST)
        {
            Data = new();
            foreach (RecurringBillModel model in models)
            {
                Data.Add(new RecurringBillDetail(model));
            }
        }
    }
}