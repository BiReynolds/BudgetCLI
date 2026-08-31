using BudgetCLI.Core.Objects;
using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Scanner.Tokens;
using Microsoft.Data.Sqlite;

namespace BudgetCLI.Evaluator.CommandHelpers
{
    public static class ShowCommandHelper
    {
        public static OutputTokenBase EvaluateShowCommand(List<BudgetTokenBase> remainingTokens, SqliteConnection connection)
        {
            if (remainingTokens[0].TokenType == BudgetTokenEnum.SUB_COMMAND)
            {
                SubCommandToken subCommandToken = remainingTokens[0] as SubCommandToken;
                switch (subCommandToken.SubCommandType)
                {
                    case SubCommandEnum.BILL:
                        return EvaluateShowBillCommand(remainingTokens[1..], connection);
                    case SubCommandEnum.BILLS:
                        GetShowBillsArgs(remainingTokens[1..]);
                        List<OneTimeBillModel> allBills = DatabaseHelper.GetAllOneTimeBills(connection);
                        return new OneTimeBillList(allBills);
                    default:
                        connection.Close();
                        throw new SubCommandNotSupportedException(BudgetMainCommandEnum.SHOW, subCommandToken.SubCommandType);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(BudgetMainCommandEnum.SHOW);
            }
        }

        static OutputTokenBase EvaluateShowBillCommand(List<BudgetTokenBase> remainingTokens, SqliteConnection connection)
        {
            GetShowBillArgs(remainingTokens[1..], out int billId);
            OneTimeBillModel? model = DatabaseHelper.GetOneTimeBillById(billId, connection);
            if (model == null)
            {
                connection.Close();
                throw new Exception($"No bill in db with id = {billId}");
            }
            else
            {
                connection.Close();
                return new SingleOneTimeBillModelDetail(model);
            }
        }

        static void GetShowBillArgs(List<BudgetTokenBase> remainingTokens, out int billId)
        {
            if (remainingTokens.Count != 1)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 1);
            }
            if (remainingTokens[0].TokenType != BudgetTokenEnum.NUMBER)
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.NUMBER);
            }
            billId = (int)((NumberToken)remainingTokens[0]).Value;
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