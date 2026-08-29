using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class OneTimeBillList : OutputTokenBase
    {
        public List<SingleOneTimeBillModelDetail> Data;
        public OneTimeBillList(List<OneTimeBillModel> bills) : base(OutputTokenEnum.ONE_TIME_BILL_LIST)
        {
            Data = new();
            foreach (OneTimeBillModel bill in bills)
            {
                Data.Add(new SingleOneTimeBillModelDetail(bill));
            }
        }
    }
}