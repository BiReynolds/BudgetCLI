using BudgetCLI.Core.Objects;
using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Session;

namespace BudgetCLI.Evaluator.CommandHelpers
{
    public static class ShowCommandHelper
    {
        public static OutputTokenBase EvaluateShowCommand(List<BudgetTokenBase> remainingTokens, SessionManager session)
        {
            if (remainingTokens[0].TokenType == BudgetTokenEnum.SUB_COMMAND)
            {
                SubCommandToken subCommandToken = remainingTokens[0] as SubCommandToken;
                switch (subCommandToken.SubCommandType)
                {
                    case SubCommandEnum.BILL:
                        return EvaluateShowBillCommand(remainingTokens[1..], session);
                    case SubCommandEnum.BILLS:
                        GetShowBillsArgs(remainingTokens[1..]);
                        IEnumerable<OneTimeBillModel> allActiveBills = FilterHelper.FilterByDeleted(session.SessionBillList, false);
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

        static OutputTokenBase EvaluateShowBillCommand(List<BudgetTokenBase> remainingTokens, SessionManager session)
        {
            OneTimeBillModel? model = GetShownBillFromArgs(remainingTokens, session);
            if (model == null)
            {
                throw new Exception($"No bill in db with criteria specified");
            }
            else
            {
                return new SingleOneTimeBillModelDetail(model);
            }

        }

        static OneTimeBillModel? GetShownBillFromArgs(List<BudgetTokenBase> remainingTokens, SessionManager session)
        {
            if (remainingTokens.Count != 1)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 1);
            }

            if (remainingTokens[0].TokenType == BudgetTokenEnum.NUMBER)
            {
                int billId = (int)((NumberToken)remainingTokens[0]).Value;
                return FilterHelper.GetById(session.SessionBillList, billId);
            }
            else if (remainingTokens[0].TokenType == BudgetTokenEnum.STRING)
            {
                string billName = ((StringToken)remainingTokens[0]).Value;
                return FilterHelper.GetByName(session.SessionBillList, billName);
            }

            throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.NUMBER);
        }

        static void GetShowBillsArgs(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count != 0)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 0);
            }
        }
    }
}