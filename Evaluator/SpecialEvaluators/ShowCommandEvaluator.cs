using BudgetCLI.Core.Objects;
using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using BudgetCLI.Evaluator.Objects;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Session;

namespace BudgetCLI.Evaluator.SpecialEvaluators
{
    public class ShowCommandEvaluator
    {
        public SessionManager Session;
        public ShowCommandEvaluator(SessionManager session)
        {
            Session = session;
        }

        public OutputTokenBase EvaluateShowCommand(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                var ReservedWordToken = (ReservedWordToken)remainingTokens[0];
                switch (ReservedWordToken.ReservedWord)
                {
                    case ReservedWordEnum.BILL:
                        return EvaluateShowBillCommand(remainingTokens[1..]);
                    case ReservedWordEnum.BILLS:
                        return EvaluateShowBillsCommand(remainingTokens[1..]);
                    case ReservedWordEnum.RECURRING:
                        return EvaluateShowRecurringCommand(remainingTokens[1..]);
                    default:
                        throw new SubCommandNotSupportedException(ReservedWordEnum.SHOW, ReservedWordToken.ReservedWord);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(ReservedWordEnum.SHOW);
            }
        }

        OutputTokenBase EvaluateShowBillCommand(List<BudgetTokenBase> remainingTokens)
        {
            OneTimeBillModel? model = EvaluateHelper.GetBillFromArgs(remainingTokens, Session);
            if (model == null)
            {
                throw new Exception($"No bill in db with criteria specified");
            }
            else
            {
                return new SingleOneTimeBillModelDetail(model);
            }

        }

        OutputTokenBase EvaluateShowBillsCommand(List<BudgetTokenBase> remainingTokens)
        {
            IEnumerable<OneTimeBillModel> filteredBills = Session.SessionBillList;
            if (remainingTokens.Count == 0)
            {
                filteredBills = filteredBills.Where(x => !x.IsPaid);
            }
            List<FilterInfo> filters = EvaluateHelper.GetAllFiltersFromArgs(remainingTokens);
            foreach (FilterInfo filter in filters)
            {
                filteredBills = EvaluateHelper.ApplyFilterToOneTimeBillModels(filteredBills, filter);
            }
            return new OneTimeBillList(filteredBills);
        }

        OutputTokenBase EvaluateShowRecurringCommand(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count == 0)
            {
                return new RecurringBillList(Session.SessionRecurringBills);
            }
            else
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 0);
            }
        }
    }
}