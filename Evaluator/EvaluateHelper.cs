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
        public static ReservedWordEnum[] ReservedWordTrueBooleanFields = [
            ReservedWordEnum.PAID,
        ];

        public static ReservedWordEnum[] ReservedWordFalseBooleanFields = [
            ReservedWordEnum.UNPAID,
        ];

        public static Dictionary<ReservedWordEnum, RecurringTypeEnum> ReservedWordToRecurringTypeDict = new()
        {
            {ReservedWordEnum.WEEKLY, RecurringTypeEnum.WEEKLY},
            {ReservedWordEnum.BIWEEKLY, RecurringTypeEnum.BIWEEKLY},
            {ReservedWordEnum.MONTHLY, RecurringTypeEnum.MONTHLY}
        };

        public static OneTimeBillModel GetBillFromArgs(List<BudgetTokenBase> remainingTokens, SessionManager session)
        {
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

        public static OneTimeBillModel CreateBillFromArgs(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count != 3)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 3);
            }
            string name = ((StringToken)remainingTokens[0]).Value;
            decimal amount = ((NumberToken)remainingTokens[1]).Value;
            DateOnly dueDate = ((DateToken)remainingTokens[2]).Value;
            return new OneTimeBillModel(name, amount, dueDate, false);
        }

        public static RecurringBillModel GetRecurringBillFromArgs(List<BudgetTokenBase> remainingTokens, SessionManager session)
        {
            if (remainingTokens.Count != 1)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 1);
            }

            RecurringBillModel? result;
            if (remainingTokens[0].TokenType == BudgetTokenEnum.STRING)
            {
                string billName = ((StringToken)remainingTokens[0]).Value;
                result = session.SessionRecurringBills.First(x => x.Name == billName);
            }
            else
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.STRING);
            }

            if (result == null)
            {
                throw new Exception($"No recurring bill in db with specified criteria");
            }
            else
            {
                return result;
            }
        }

        public static RecurringBillModel CreateRecurringBillFromArgs(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens[3].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                
                string name = ((StringToken)remainingTokens[0]).Value;
                decimal amount = ((NumberToken)remainingTokens[1]).Value;
                DateOnly startDate = ((DateToken)remainingTokens[2]).Value;
                if (!ReservedWordToRecurringTypeDict.TryGetValue(((ReservedWordToken)remainingTokens[3]).ReservedWord, out RecurringTypeEnum recurringType))
                {
                    throw new Exception($"Expected a Recurring Type, received {((ReservedWordToken)remainingTokens[3]).ReservedWord}");
                }
                if (remainingTokens.Count == 4)
                {
                    return new RecurringBillModel(name, amount, startDate, recurringType);
                }
                else if (remainingTokens[4].TokenType == BudgetTokenEnum.DATE)
                {
                    DateOnly endDate = ((DateToken)remainingTokens[4]).Value;
                    return new RecurringBillModel(name, amount, startDate, endDate, recurringType);
                }
                else
                {
                    throw new UnexpectedArgTypeException(remainingTokens[4], BudgetTokenEnum.DATE);
                }
            }
            else
            {
                throw new UnexpectedArgTypeException(remainingTokens[3], BudgetTokenEnum.RESERVED_WORD);
            }
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
                    return FilterHelper.FilterDataWithBooleanFilter(data, x => x.IsPaid, booleanFilter);
                case ReservedWordEnum.AMOUNT:
                    var numberFilter = (NumberFilterInfo)filter;
                    return FilterHelper.FilterDataWithNumberFilter(data, x => x.Amount, numberFilter);
                case ReservedWordEnum.NAME:
                    var stringFilter = (StringFilterInfo)filter;
                    return FilterHelper.FilterDataWithStringFilter(data, x => x.Name, stringFilter);
                case ReservedWordEnum.DUE_DATE:
                    var dateFilter = (DateFilterInfo)filter;
                    return FilterHelper.FilterDataWithDateFilter(data, x => x.DueDate, dateFilter);
                default:
                    throw new FilterSyntaxError();
            }
        }
    }
}