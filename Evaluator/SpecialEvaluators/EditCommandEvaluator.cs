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
                case ReservedWordEnum.RECURRING:
                    return EvaluateEditRecurringCommand(remainingTokens[1..]);
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
                    throw new SubCommandNotSupportedException([ReservedWordEnum.EDIT, ReservedWordEnum.BILL], editedField.ReservedWord);
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

        OutputTokenBase EvaluateEditRecurringCommand(List<BudgetTokenBase> remainingTokens)
        {
            EditRecurringCommandArgTypeCheck(remainingTokens);
            RecurringBillModel model = EvaluateHelper.GetRecurringBillFromArgs(remainingTokens[0..1], Session);
            ReservedWordToken editedField = (ReservedWordToken)remainingTokens[1];
            SimpleTextOutput result;
            switch (editedField.ReservedWord)
            {
                case ReservedWordEnum.NAME:
                    StringToken newNameToken = (StringToken)remainingTokens[2];
                    result = new($"Recurring Bill {model.Name} has been renamed to {newNameToken.Value}");
                    model.Name = newNameToken.Value;
                    UpdateAllInstanceNames(model);
                    return result;
                case ReservedWordEnum.AMOUNT:
                    NumberToken newAmountToken = (NumberToken)remainingTokens[2];
                    result = new($"Recurring Bill {model.Name} amount changed from {model.Amount} to {newAmountToken.Value}");
                    model.Amount = newAmountToken.Value;
                    UpdateAllInstanceAmounts(model);
                    return result;
                case ReservedWordEnum.NEXT_DUE:
                    DateToken nextDueDateToken = (DateToken)remainingTokens[2];
                    model.ReferenceDate = nextDueDateToken.Value;
                    ApplyNextDueDateToInstances(model, out DateOnly newLastDueDateAdded, out DateOnly prevNextDueDate);
                    model.LastOneTimeDueDateAdded = newLastDueDateAdded;
                    result = new($"Recurring Bill {model.Name} next due date changed from {prevNextDueDate} to {nextDueDateToken.Value}");
                    return result;
                case ReservedWordEnum.END_DATE:
                    DateToken endDateToken = (DateToken)remainingTokens[2];
                    DateOnly? oldEndDate = model.EndDate;
                    model.EndDate = endDateToken.Value;
                    ApplyEndDateToInstances(model, oldEndDate, out DateOnly newLastInstanceAddedDate);
                    model.LastOneTimeDueDateAdded = newLastInstanceAddedDate;
                    result = new($"Recurring Bill {model.Name} end date changed from {oldEndDate} to {model.EndDate}");
                    return result;
                default:
                    throw new SubCommandNotSupportedException([ReservedWordEnum.EDIT, ReservedWordEnum.RECURRING], editedField.ReservedWord);
            }
           
        }

        void EditRecurringCommandArgTypeCheck(List<BudgetTokenBase> remainingTokens)
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

        void UpdateAllInstanceNames(RecurringBillModel recurringModel)
        {
            IEnumerable<OneTimeBillModel> unpaidInstances = Session.SessionBillList.Where(x => x.ParentId == recurringModel.Id && !x.IsPaid);
            foreach (var instance in unpaidInstances)
            {
                instance.Name = recurringModel.Name + ' ' + instance.DueDate;
            }
        }

        void UpdateAllInstanceAmounts(RecurringBillModel recurringModel)
        {
            IEnumerable<OneTimeBillModel> unpaidInstances = Session.SessionBillList.Where(x => x.ParentId == recurringModel.Id && !x.IsPaid);
            foreach (var instance in unpaidInstances)
            {
                instance.Amount= recurringModel.Amount;
            }
        }

        void ApplyNextDueDateToInstances(RecurringBillModel recurringModel, out DateOnly newLastDueDateAdded, out DateOnly prevNextDueDate)
        {
            IEnumerable<OneTimeBillModel> unpaidInstances = Session.SessionBillList.Where(x => x.ParentId == recurringModel.Id && !x.IsPaid);
            prevNextDueDate = unpaidInstances.Min(x => x.DueDate);
            int dayDiff = (int)(recurringModel.ReferenceDate.ToDateTime(TimeOnly.MinValue) - prevNextDueDate.ToDateTime(TimeOnly.MinValue)).TotalDays;
            foreach (var instance in unpaidInstances)
            {
                instance.DueDate = instance.DueDate.AddDays(dayDiff);
                instance.Name = recurringModel.Name + ' ' + instance.DueDate;
            }
            newLastDueDateAdded = unpaidInstances.Max(x => x.DueDate);
        }

        void ApplyEndDateToInstances(RecurringBillModel recurringModel, DateOnly? oldEndDate, out DateOnly newLastInstanceAddedDate)
        {
            if (recurringModel.EndDate < oldEndDate)
            {
                IEnumerable<OneTimeBillModel> unpaidInstances = Session.SessionBillList.Where(x => x.ParentId == recurringModel.Id && !x.IsPaid);
                foreach (var instance in unpaidInstances)
                {
                    if (instance.DueDate > recurringModel.EndDate)
                    {
                        instance.IsDeleted = true;
                    }
                }
            }
            else
            {
                IEnumerable<OneTimeBillModel> newInstances = recurringModel.GetNewBillInstances(Session.Today.AddMonths(12));
                Session.SessionBillList.AddRange(newInstances);
            }
            newLastInstanceAddedDate = Session.SessionBillList.Where(x => !x.IsDeleted).Max(x => x.DueDate);
        }
    }
}