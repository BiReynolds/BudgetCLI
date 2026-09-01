using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;
using BudgetCLI.Evaluator.Objects;
using BudgetCLI.Exceptions;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Evaluator
{
    public static class FilterHelper<T>
    {
        public static IEnumerable<T> FilterDataWithBooleanFilter(IEnumerable<T> data, Func<T, bool> selector, BooleanFilterInfo booleanFilter)
        {
            switch (booleanFilter.ComparatorToken.ReservedWord)
            {
                case ReservedWordEnum.EQUAL:
                    return data.Where(x => selector(x) == booleanFilter.FilterValue);
                default:
                    throw new ComparatorNotSupportedException(BudgetTokenEnum.BOOLEAN, booleanFilter.ComparatorToken.ReservedWord);
            }
        }

        public static IEnumerable<T> FilterDataWithNumberFilter(IEnumerable<T> data, Func<T, decimal> selector, NumberFilterInfo numberFilter)
        {
            switch (numberFilter.ComparatorToken.ReservedWord)
            {
                case ReservedWordEnum.EQUAL:
                    return data.Where(x => selector(x) == numberFilter.FilterValue);
                case ReservedWordEnum.LESS_THAN:
                    return data.Where(x => selector(x) < numberFilter.FilterValue);
                case ReservedWordEnum.GREATER_THAN:
                    return data.Where(x => selector(x) > numberFilter.FilterValue);
                case ReservedWordEnum.LESS_OR_EQUAL:
                    return data.Where(x => selector(x) <= numberFilter.FilterValue);
                case ReservedWordEnum.GREATER_OR_EQUAL:
                    return data.Where(x => selector(x) >= numberFilter.FilterValue);
                default:
                    throw new ComparatorNotSupportedException(BudgetTokenEnum.NUMBER, numberFilter.ComparatorToken.ReservedWord);
            }
        }

        public static IEnumerable<T> FilterDataWithStringFilter(IEnumerable<T> data, Func<T, string> selector, StringFilterInfo stringFilter)
        {
            switch (stringFilter.ComparatorToken.ReservedWord)
            {
                case ReservedWordEnum.EQUAL:
                    return data.Where(x => selector(x) == stringFilter.FilterValue);
                case ReservedWordEnum.CONTAINS:
                    return data.Where(x => selector(x).Contains(stringFilter.FilterValue));
                default:
                    throw new ComparatorNotSupportedException(BudgetTokenEnum.STRING, stringFilter.ComparatorToken.ReservedWord);
            }
        }

        public static IEnumerable<T> FilterDataWithDateFilter(IEnumerable<T> data, Func<T, DateOnly> selector, DateFilterInfo dateFilter)
        {
            switch (dateFilter.ComparatorToken.ReservedWord)
            {
                case ReservedWordEnum.EQUAL:
                    return data.Where(x => selector(x) == dateFilter.FilterValue);
                case ReservedWordEnum.LESS_THAN:
                    return data.Where(x => selector(x) < dateFilter.FilterValue);
                case ReservedWordEnum.GREATER_THAN:
                    return data.Where(x => selector(x) > dateFilter.FilterValue);
                case ReservedWordEnum.LESS_OR_EQUAL:
                    return data.Where(x => selector(x) <= dateFilter.FilterValue);
                case ReservedWordEnum.GREATER_OR_EQUAL:
                    return data.Where(x => selector(x) >= dateFilter.FilterValue);
                default:
                    throw new ComparatorNotSupportedException(BudgetTokenEnum.NUMBER, dateFilter.ComparatorToken.ReservedWord);
            }
        }
    }
}