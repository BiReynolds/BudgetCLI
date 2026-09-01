using BudgetCLI.Core.Objects;
using BudgetCLI.Data;
using BudgetCLI.Data.Models;
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
            if (remainingTokens[0].TokenType == BudgetTokenEnum.SUB_COMMAND)
            {
                SubCommandToken subCommandToken = remainingTokens[0] as SubCommandToken;
                switch (subCommandToken.SubCommandType)
                {
                    case SubCommandEnum.BILL:
                        return EvaluateShowBillCommand(remainingTokens[1..]);
                    case SubCommandEnum.BILLS:
                        GetShowBillsArgs(remainingTokens[1..]);
                        IEnumerable<OneTimeBillModel> allActiveBills = FilterHelper.FilterByDeleted(Session.SessionBillList, false);
                        return new OneTimeBillList(allActiveBills);
                    default:
                        throw new SubCommandNotSupportedException(BudgetMainCommandEnum.SHOW, subCommandToken.SubCommandType);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(BudgetMainCommandEnum.SHOW);
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

        void GetShowBillsArgs(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count != 0)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 0);
            }
        }
    }
}