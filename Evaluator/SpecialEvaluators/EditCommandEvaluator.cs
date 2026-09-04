using BudgetCLI.Core.Objects;
using BudgetCLI.Data.Models;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Session;

namespace BudgetCLI.Evaluator.SpecialEvaluators
{
    public class EditCommandEvaluator
    {
        public SessionManager Session;
        public EditCommandEvaluator(SessionManager session)
        {
            Session = session;
        }
        public OutputTokenBase EvaluateEditCommand(List<BudgetTokenBase> remainingTokens)
        {
            ReservedWordToken editedObjectType = (ReservedWordToken)remainingTokens[0];
            switch (editedObjectType.ReservedWord)
            {
                case ReservedWordEnum.BILL:
                    return EvaluateEditBillCommand(remainingTokens[1..]);
                case ReservedWordEnum.BALANCE:
                    return EvaluateEditBalanceCommand(remainingTokens[1..]);
                default:
                    throw new SubCommandNotSupportedException(ReservedWordEnum.EDIT, editedObjectType.ReservedWord);
            }
        }

        OutputTokenBase EvaluateEditBalanceCommand(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count != 1)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 1);
            }
            else if (remainingTokens[0].TokenType == BudgetTokenEnum.NUMBER)
            {
                var newBalanceToken = (NumberToken)remainingTokens[0];
                Session.SessionBalance = newBalanceToken.Value;
                return new SimpleTextOutput($"Session Balance updated to {Session.SessionBalance}");
            }
            else
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.NUMBER);
            }
        }

        OutputTokenBase EvaluateEditBillCommand(List<BudgetTokenBase> remainingTokens)
        {
            EditBillCommandArgTypeCheck(remainingTokens);
            OneTimeBillModel model = EvaluateHelper.GetBillFromArgs(remainingTokens[0..1], Session);
            ReservedWordToken editedField = (ReservedWordToken)remainingTokens[1];
            SimpleTextOutput result;
            switch (editedField.ReservedWord)
            {
                case ReservedWordEnum.NAME:
                    StringToken newNameToken = (StringToken)remainingTokens[2];
                    result = new($"Bill {model.Name} has been renamed to {newNameToken.Value}");
                    model.Name = newNameToken.Value;
                    return result;
                case ReservedWordEnum.AMOUNT:
                    NumberToken newAmountToken = (NumberToken)remainingTokens[2];
                    result = new($"Bill {model.Name} amount changed from {model.Amount} to {newAmountToken.Value}");
                    model.Amount = newAmountToken.Value;
                    return result;
                case ReservedWordEnum.DUE_DATE:
                    DateToken newDateToken = (DateToken)remainingTokens[2];
                    result = new($"Bill {model.Name} due date changed from {model.DueDate} to {newDateToken.Value}");
                    model.DueDate = newDateToken.Value;
                    return result;
                default:
                    // shouldn't be possible due to EditCommandArgType check, but whatever
                    throw new UnexpectedArgTypeException(remainingTokens[2], [BudgetTokenEnum.NUMBER, BudgetTokenEnum.STRING, BudgetTokenEnum.DATE]);
            }
        }

        void EditBillCommandArgTypeCheck(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count != 3)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 3);
            }
            if (remainingTokens[1].TokenType != BudgetTokenEnum.RESERVED_WORD)
            {
                throw new UnexpectedArgTypeException(remainingTokens[1], BudgetTokenEnum.RESERVED_WORD);
            }
            List<BudgetTokenEnum> acceptedTypes = [BudgetTokenEnum.NUMBER, BudgetTokenEnum.STRING, BudgetTokenEnum.DATE];
            if (!acceptedTypes.Contains(remainingTokens[2].TokenType))
            {
                throw new UnexpectedArgTypeException(remainingTokens[2], acceptedTypes);
            }
        }

    }
}