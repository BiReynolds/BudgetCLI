using BudgetCLI.Data.Models;

namespace BudgetCLI.Data
{
    public static class FilterHelper
    {
        public static OneTimeBillModel? GetById(IEnumerable<OneTimeBillModel> billList, int id)
        {
            try
            {
                return billList.First(
                    x => x.Id == id
                );
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public static IEnumerable<OneTimeBillModel> FilterByDeleted(IEnumerable<OneTimeBillModel> billList, bool testIsDeleted)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.IsDeleted == testIsDeleted
            );
            return result;
        }

        public static IEnumerable<OneTimeBillModel> FilterByPaid(IEnumerable<OneTimeBillModel> billList, bool testIsPaid)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.IsPaid == testIsPaid
            );
            return result;
        }

        public static IEnumerable<OneTimeBillModel> FilterByDueBefore(IEnumerable<OneTimeBillModel> billList, DateOnly testDate)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.DueDate <= testDate
            );
            return result;
        }

        public static IEnumerable<OneTimeBillModel> FilterByDueAfter(IEnumerable<OneTimeBillModel> billList, DateOnly testDate)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.DueDate >= testDate
            );
            return result;
        }

        public static IEnumerable<OneTimeBillModel> FilterByDueOn(IEnumerable<OneTimeBillModel> billList, DateOnly testDate)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.DueDate == testDate
            );
            return result;
        }

        public static IEnumerable<OneTimeBillModel> FilterByNameContains(IEnumerable<OneTimeBillModel> billList, string testString)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.Name.Contains(testString)
            );
            return result;
        }

        public static IEnumerable<OneTimeBillModel> FilterByAmountLEQ(IEnumerable<OneTimeBillModel> billList, decimal testAmount)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.Amount <= testAmount
            );
            return result;
        }

        public static IEnumerable<OneTimeBillModel> FilterByAmountGEQ(IEnumerable<OneTimeBillModel> billList, decimal testAmount)
        {
            IEnumerable<OneTimeBillModel> result = billList.Where(
                x => x.Amount >= testAmount
            );
            return result;
        }
    }
}