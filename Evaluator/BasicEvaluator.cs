using Microsoft.Data.Sqlite;
using BudgetCLI.Core.Objects;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Data;
using BudgetCLI.Data.Models;

namespace BudgetCLI.Evaluator 
{
    public class BasicEvaluator : IEvaluator
    {
        SqliteConnection Connection = DatabaseHelper.GetReadWriteConnection();
        public OutputTokenBase Evaluate(List<BudgetTokenBase> tokens)
        {
            BudgetTokenBase firstToken = tokens[0];
            if (firstToken.TokenType != BudgetTokenEnum.MAIN_COMMAND)
            {
                throw new NoLeadingCommandException(firstToken);
            }
            if (firstToken is not MainCommandToken commandToken)
            {
                throw new TokenTypeMismatchException(BudgetTokenEnum.MAIN_COMMAND, typeof(MainCommandToken));
            }
            switch (commandToken.CommandType)
            {
                case BudgetMainCommandEnum.SHOW:
                    return EvaluateShowCommand(tokens[1..]);
                case BudgetMainCommandEnum.ADD:
                    return EvaluateAddCommand(tokens[1..]);
                case BudgetMainCommandEnum.DELETE:
                    return EvaluateDeleteCommand(tokens[1..]);
                default:
                    throw new CommandNotSupportedException(commandToken);
            }
        }

        OutputTokenBase EvaluateShowCommand(List<BudgetTokenBase> remainingTokens)
        {
            Connection.Open();
            if (remainingTokens[0].TokenType == BudgetTokenEnum.SUB_COMMAND)
            {
                SubCommandToken subCommandToken = remainingTokens[0] as SubCommandToken;
                switch (subCommandToken.SubCommandType)
                {
                    case SubCommandEnum.BILL:
                        GetShowBillArgs(remainingTokens[1..], out int billId);
                        OneTimeBillModel? model = DatabaseHelper.GetOneTimeBillById(billId, Connection);
                        if (model == null)
                        {
                            Connection.Close();
                            return new ErrorTextOutput($"No bill in db with id = {billId}");
                        }
                        else
                        {
                            Connection.Close();
                            return new SingleOneTimeBillModelDetail(model);
                        }
                    case SubCommandEnum.BILLS:
                        GetShowBillsArgs(remainingTokens[1..]);
                        List<OneTimeBillModel> allBills = DatabaseHelper.GetAllOneTimeBills(Connection);
                        return new OneTimeBillList(allBills);
                    default:
                        Connection.Close();
                        throw new SubCommandNotSupportedException(BudgetMainCommandEnum.SHOW, subCommandToken.SubCommandType);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(BudgetMainCommandEnum.SHOW);
            }
        }

        void GetShowBillArgs(List<BudgetTokenBase> remainingTokens, out int billId)
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

        void GetShowBillsArgs(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count != 0)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 0);
            }
        }

        OutputTokenBase EvaluateAddCommand(List<BudgetTokenBase> remainingTokens)
        {
            Connection.Open();
            if (remainingTokens[0].TokenType == BudgetTokenEnum.SUB_COMMAND)
            {
                SubCommandToken subCommandToken = remainingTokens[0] as SubCommandToken;
                switch (subCommandToken.SubCommandType)
                {
                    case SubCommandEnum.BILL:
                        GetAddBillArgs(remainingTokens[1..], out string name, out decimal amount, out DateOnly dueDate);
                        OneTimeBillModel model = new(name, amount, dueDate, false);
                        DatabaseHelper.AddOneTimeBillToDatabase(model, Connection);
                        Connection.Close();
                        return new SimpleTextOutput($"Bill {name} added to database");
                    default:
                        Connection.Close();
                        throw new SubCommandNotSupportedException(BudgetMainCommandEnum.ADD, subCommandToken.SubCommandType);
                }
            }
            else
            {
                Connection.Close();
                throw new ExpectedSubCommandException(BudgetMainCommandEnum.ADD);
            }
        }

        void GetAddBillArgs(List<BudgetTokenBase> remainingTokens, out string name, out decimal amount, out DateOnly dueDate)
        {
            if (remainingTokens.Count != 3)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 3);
            }
            if (remainingTokens[0].TokenType != BudgetTokenEnum.STRING)
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.STRING);
            }
            if (remainingTokens[1].TokenType != BudgetTokenEnum.NUMBER)
            {
                throw new UnexpectedArgTypeException(remainingTokens[1], BudgetTokenEnum.NUMBER);
            }
            if (remainingTokens[2].TokenType != BudgetTokenEnum.DATE)
            {
                throw new UnexpectedArgTypeException(remainingTokens[2], BudgetTokenEnum.DATE);
            }
            name = ((StringToken)remainingTokens[0]).Value;
            amount = ((NumberToken)remainingTokens[1]).Value;
            dueDate = ((DateToken)remainingTokens[2]).Value;
        }

        OutputTokenBase EvaluateDeleteCommand(List<BudgetTokenBase> remainingTokens)
        {
            Connection.Open();
            if (remainingTokens[0].TokenType == BudgetTokenEnum.SUB_COMMAND)
            {
                SubCommandToken subCommandToken = remainingTokens[0] as SubCommandToken;
                switch (subCommandToken.SubCommandType)
                {
                    case SubCommandEnum.BILL:
                        GetDeleteBillArgs(remainingTokens[1..], out int billId);
                        bool didDelete = DatabaseHelper.DeleteOneTimeBillById(billId, Connection);
                        if (didDelete)
                        {
                            return new SimpleTextOutput($"bill with id {billId} deleted");
                        }
                        else 
                        {
                            return new SimpleTextOutput($"no change made - no bill found with id {billId}");
                        }
                    default:
                        throw new SubCommandNotSupportedException(BudgetMainCommandEnum.DELETE, subCommandToken.SubCommandType);
                }
            }
            else
            {
                Connection.Close();
                throw new ExpectedSubCommandException(BudgetMainCommandEnum.ADD);
            }
        }

        void GetDeleteBillArgs(List<BudgetTokenBase> remainingTokens, out int billId)
        {
            if (remainingTokens.Count != 1)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count - 1, 1);
            }
            if (remainingTokens[0].TokenType != BudgetTokenEnum.NUMBER)
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.NUMBER);
            }
            billId = (int)((NumberToken)remainingTokens[0]).Value;
        }
    }
}