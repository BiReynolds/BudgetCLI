using BudgetCLI.Core.Objects;
using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using BudgetCLI.Exceptions;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Session;

namespace BudgetCLI.Evaluator
{
    public static class EvaluateHelper
    {
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
                result = FilterHelper.GetById(session.SessionBillList, billId);
            }
            else if (remainingTokens[0].TokenType == BudgetTokenEnum.STRING)
            {
                string billName = ((StringToken)remainingTokens[0]).Value;
                result = FilterHelper.GetByName(session.SessionBillList, billName);
            }
            else
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.NUMBER);
            }

            if (result == null)
            {
                throw new Exception($"No bill in db with specified criteria");
            }
            else {
                return result;
            }
        }


    }
}