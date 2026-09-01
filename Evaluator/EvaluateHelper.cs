using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;
using BudgetCLI.Exceptions;
using BudgetCLI.Evaluator.Objects;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Session;

namespace BudgetCLI.Evaluator
{
    public static class EvaluateHelper
    {
        public static ReservedWordEnum[] ReservedWordComparators = [
            ReservedWordEnum.LESS_THAN,
            ReservedWordEnum.GREATER_THAN,
            ReservedWordEnum.EQUAL,
            ReservedWordEnum.LESS_OR_EQUAL,
            ReservedWordEnum.GREATER_OR_EQUAL,
            ReservedWordEnum.CONTAINS
        ];

        public static ReservedWordEnum[] ReservedWordTrueBooleanFields = [
            ReservedWordEnum.PAID,
        ];

        public static ReservedWordEnum[] ReservedWordFalseBooleanFields = [
            ReservedWordEnum.UNPAID,
        ];

        public static ReservedWordEnum[] ReservedWordOneTimeBillFields = [
            ReservedWordEnum.NAME,
            ReservedWordEnum.AMOUNT,
            ReservedWordEnum.DUE_DATE,
            ReservedWordEnum.PAID,
            ReservedWordEnum.UNPAID
        ];

        public static OneTimeBillModel GetBillFromArgs(List<BudgetTokenBase> remainingTokens, SessionManager session)
        {
            // Exact same method as "ShowCommandHelper.GetShownBillFromArgs," and will likely also be used for other commands... but will need a slight refactor of various methods
            if (remainingTokens.Count != 1)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 1);
            }

            OneTimeBillModel? result;
            if (remainingTokens[0].TokenType == BudgetTokenEnum.NUMBER)
            {
                int billId = (int)((NumberToken)remainingTokens[0]).Value;
                result = session.SessionBillList.First(x => x.Id == billId);
            }
            else if (remainingTokens[0].TokenType == BudgetTokenEnum.STRING)
            {
                string billName = ((StringToken)remainingTokens[0]).Value;
                result = session.SessionBillList.First(x => x.Name == billName);
            }
            else
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], [BudgetTokenEnum.NUMBER, BudgetTokenEnum.STRING]);
            }

            if (result == null)
            {
                throw new Exception($"No bill in db with specified criteria");
            }
            else 
            {
                return result;
            }
        }

        public static bool IsOneTimeBillField(ReservedWordToken testToken)
        {
            return ReservedWordOneTimeBillFields.Contains(testToken.ReservedWord);
        }

        public static bool IsComparator(ReservedWordToken testToken)
        {
            return ReservedWordComparators.Contains(testToken.ReservedWord);
        }

        public static bool IsTrueBooleanField(ReservedWordToken testToken)
        {
            return ReservedWordTrueBooleanFields.Contains(testToken.ReservedWord);
        }

        public static bool IsFalseBooleanField(ReservedWordToken testToken)
        {
            return ReservedWordFalseBooleanFields.Contains(testToken.ReservedWord);
        }

        public static List<FilterInfo> GetAllFiltersFromArgs(List<BudgetTokenBase> remainingTokens)
        {
            List<FilterInfo> result = new();
            BudgetTokenBase[] leftoverTokens = remainingTokens.ToArray();
            int leftoverTokensStartIndex;
            while (leftoverTokens.Length > 0)
            {
                result.Add(ReadNextFilterInfo(leftoverTokens, out leftoverTokensStartIndex));
                leftoverTokens = leftoverTokens[leftoverTokensStartIndex..];
            }
            return result;
        }


        static FilterInfo ReadNextFilterInfo(BudgetTokenBase[] remainingTokens, out int nextLeftoverTokensStartIndex)
        {
            if (remainingTokens.Length == 0)
            {
                throw new Exception("Cannot call GetNextFilterInfo on an empty array");
            }

            if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                var fieldToken = (ReservedWordToken)remainingTokens[0];
                // Special syntax for boolean fields (instead of writing `paid = true`, we instead just write `paid`)
                if (IsTrueBooleanField(fieldToken))
                {
                    nextLeftoverTokensStartIndex = 1;
                    return new BooleanFilterInfo(fieldToken, true);
                }
                else if (IsFalseBooleanField(fieldToken))
                {
                    nextLeftoverTokensStartIndex = 1;
                    return new BooleanFilterInfo(fieldToken, false);
                }
                // This handles all other cases, where syntax is `reservedword reservedword anytoken`
                else if (remainingTokens[1].TokenType == BudgetTokenEnum.RESERVED_WORD)
                {
                    nextLeftoverTokensStartIndex = 3;
                    var comparatorToken = (ReservedWordToken)remainingTokens[1];
                    if (remainingTokens[2].TokenType == BudgetTokenEnum.NUMBER)
                    {
                        return new NumberFilterInfo(fieldToken, comparatorToken, ((NumberToken)remainingTokens[2]));
                    }
                    else if (remainingTokens[2].TokenType == BudgetTokenEnum.STRING)
                    {
                        return new StringFilterInfo(fieldToken, comparatorToken, ((StringToken)remainingTokens[2]));
                    }
                    else if (remainingTokens[2].TokenType == BudgetTokenEnum.DATE)
                    {
                        return new DateFilterInfo(fieldToken, comparatorToken, ((DateToken)remainingTokens[2]));
                    }
                }
            }
            // if we got here, we didn't know how to handle the filter...
            throw new FilterSyntaxError();
        }

        public static IEnumerable<OneTimeBillModel> ApplyFilterToOneTimeBillModels(IEnumerable<OneTimeBillModel> data, FilterInfo filter)
        {
            switch (filter.FieldToken.ReservedWord)
            {
                case ReservedWordEnum.PAID:
                case ReservedWordEnum.UNPAID:
                    var booleanFilter = (BooleanFilterInfo)filter;
                    return FilterHelper<OneTimeBillModel>.FilterDataWithBooleanFilter(data, x => x.IsPaid, booleanFilter);
                case ReservedWordEnum.AMOUNT:
                    var numberFilter = (NumberFilterInfo)filter;
                    return FilterHelper<OneTimeBillModel>.FilterDataWithNumberFilter(data, x => x.Amount, numberFilter);
                case ReservedWordEnum.NAME:
                    var stringFilter = (StringFilterInfo)filter;
                    return FilterHelper<OneTimeBillModel>.FilterDataWithStringFilter(data, x => x.Name, stringFilter);
                case ReservedWordEnum.DUE_DATE:
                    var dateFilter = (DateFilterInfo)filter;
                    return FilterHelper<OneTimeBillModel>.FilterDataWithDateFilter(data, x => x.DueDate, dateFilter);
                default:
                    throw new FilterSyntaxError();
            }
        }
    }
}