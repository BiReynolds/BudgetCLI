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
            SubCommandToken editedObjectType = (SubCommandToken)remainingTokens[0];
            switch (editedObjectType.SubCommandType)
            {
                case SubCommandEnum.BILL:
                    return EvaluateEditBillCommand(remainingTokens[1..]);
                default:
                    throw new SubCommandNotSupportedException(BudgetMainCommandEnum.EDIT, editedObjectType.SubCommandType);
            }
        }
        OutputTokenBase EvaluateEditBillCommand(List<BudgetTokenBase> remainingTokens)
        {
            EditBillCommandArgTypeCheck(remainingTokens);
            OneTimeBillModel model = EvaluateHelper.GetBillFromArgs(remainingTokens[0..1], Session);
            SubCommandToken editedField = (SubCommandToken)remainingTokens[1];
            SimpleTextOutput result;
            switch (editedField.SubCommandType)
            {
                case SubCommandEnum.NAME:
                    StringToken newNameToken = (StringToken)remainingTokens[2];
                    result = new($"Bill {model.Name} has been renamed to {newNameToken.Value}");
                    model.Name = newNameToken.Value;
                    return result;
                case SubCommandEnum.AMOUNT:
                    NumberToken newAmountToken = (NumberToken)remainingTokens[2];
                    result = new($"Bill {model.Name} amount changed from {model.Amount} to {newAmountToken.Value}");
                    model.Amount = newAmountToken.Value;
                    return result;
                case SubCommandEnum.DUE_DATE:
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
            if (remainingTokens[1].TokenType != BudgetTokenEnum.SUB_COMMAND)
            {
                throw new UnexpectedArgTypeException(remainingTokens[1], BudgetTokenEnum.SUB_COMMAND);
            }
            List<BudgetTokenEnum> acceptedTypes = [BudgetTokenEnum.NUMBER, BudgetTokenEnum.STRING, BudgetTokenEnum.DATE];
            if (!acceptedTypes.Contains(remainingTokens[2].TokenType))
            {
                throw new UnexpectedArgTypeException(remainingTokens[2], acceptedTypes);
            }
        }

    }
}