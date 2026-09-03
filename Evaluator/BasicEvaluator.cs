using BudgetCLI.Core.Objects;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Data.Models;
using BudgetCLI.Session;
using BudgetCLI.Evaluator.SpecialEvaluators;

namespace BudgetCLI.Evaluator 
{
    public class BasicEvaluator : IEvaluator
    {
        public event EventHandler? SafeExitEvent;
        public event EventHandler? UnsavedChangesExitEvent;
        SessionManager? Session;
        ShowCommandEvaluator? ShowCommandEvaluator;
        EditCommandEvaluator? EditCommandEvaluator;
        public void SetSessionData(SessionManager session)
        {
            Session = session;
            ShowCommandEvaluator = new(session);
            EditCommandEvaluator = new(session);
        }

        public OutputTokenBase Evaluate(List<BudgetTokenBase> tokens)
        {
            if (Session == null)
            {
                throw new Exception("Session data is null at Evaluate - did you call SetSessionData?");
            }
            BudgetTokenBase firstToken = tokens[0];
            if (firstToken.TokenType == BudgetTokenEnum.STRING)
            {
                return EvaluateLeadingStringCommand((StringToken)firstToken, tokens[1..]);
            }
            else if (firstToken.TokenType == BudgetTokenEnum.RESERVED_WORD) {
                ReservedWordToken commandToken = (ReservedWordToken)firstToken;
                switch (commandToken.ReservedWord)
                {
                    case ReservedWordEnum.EXIT:
                        return EvaluateExitCommand();
                    case ReservedWordEnum.SHOW:
                        return ShowCommandEvaluator.EvaluateShowCommand(tokens[1..]);
                    case ReservedWordEnum.ADD:
                        return EvaluateAddCommand(tokens[1..]);
                    case ReservedWordEnum.DELETE:
                        return EvaluateDeleteCommand(tokens[1..]);
                    case ReservedWordEnum.PAID:
                        return EvaluatePaidCommand(tokens[1..]);
                    case ReservedWordEnum.UNPAID:
                        return EvaluateUnpaidCommand(tokens[1..]);
                    case ReservedWordEnum.EDIT:
                        return EditCommandEvaluator.EvaluateEditCommand(tokens[1..]);
                    case ReservedWordEnum.SAVE:
                        return EvaluateSaveCommand();
                    case ReservedWordEnum.RESET:
                        return EvaluateResetCommand();
                    default:
                        throw new CommandNotSupportedException(commandToken);
                }
            }

            else
            {
                throw new NoLeadingCommandException(firstToken);
            }
        }

        OutputTokenBase EvaluateExitCommand()
        {
            if (Session.UnsavedChanges)
            {
                OnUnsavedChangesExitEvent(EventArgs.Empty);
                return new ExitNotification(false);
            }
            else
            {
                OnSafeExitEvent(EventArgs.Empty);
                return new ExitNotification(true);
            }
        }

        OutputTokenBase EvaluateLeadingStringCommand(StringToken leadingStringToken, List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count != 0)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 0);
            }

            return ShowCommandEvaluator.EvaluateShowCommand([new ReservedWordToken("bill", ReservedWordEnum.BILL), leadingStringToken]);
        }

        OutputTokenBase EvaluateAddCommand(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count < 4 || remainingTokens.Count > 6)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, [4, 5, 6]);
            }
            if (remainingTokens[1].TokenType != BudgetTokenEnum.STRING)
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], BudgetTokenEnum.STRING);
            }
            if (remainingTokens[2].TokenType != BudgetTokenEnum.NUMBER)
            {
                throw new UnexpectedArgTypeException(remainingTokens[1], BudgetTokenEnum.NUMBER);
            }
            if (remainingTokens[3].TokenType != BudgetTokenEnum.DATE)
            {
                throw new UnexpectedArgTypeException(remainingTokens[2], BudgetTokenEnum.DATE);
            }

            if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                ReservedWordToken ReservedWordToken = remainingTokens[0] as ReservedWordToken;
                switch (ReservedWordToken.ReservedWord)
                {
                    case ReservedWordEnum.BILL:
                        if (remainingTokens.Count == 4)
                        {
                            OneTimeBillModel billModel = EvaluateHelper.CreateBillFromArgs(remainingTokens[1..]);
                            Session.AddNewOneTimeBill(billModel);
                            return new SimpleTextOutput($"Bill {billModel.Name} added");
                        }
                        else
                        {
                            RecurringBillModel recurringBillModel = EvaluateHelper.CreateRecurringBillFromArgs(remainingTokens[1..]);
                            Session.AddNewRecurringBill(recurringBillModel);
                            Session.SaveSession();
                            recurringBillModel = Session.GetRecurringBillModelByName(recurringBillModel.Name);
                            List<OneTimeBillModel> recurringBillInstances = recurringBillModel.GetNewBillInstances(DateOnly.FromDateTime(DateTime.Today).AddMonths(12));
                            Session.AddManyOneTimeBills(recurringBillInstances);
                            return new SimpleTextOutput($"Recurring bill {recurringBillModel.Name} and next year of instances added");
                        }
                    default:
                        throw new SubCommandNotSupportedException(ReservedWordEnum.ADD, ReservedWordToken.ReservedWord);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(ReservedWordEnum.ADD);
            }
        }

        OutputTokenBase EvaluateDeleteCommand(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                ReservedWordToken ReservedWordToken = remainingTokens[0] as ReservedWordToken;
                switch (ReservedWordToken.ReservedWord)
                {
                    case ReservedWordEnum.BILL:
                        OneTimeBillModel deletedBill = EvaluateHelper.GetBillFromArgs(remainingTokens[1..], Session);
                        Session.DeleteOneTimeBill(deletedBill);
                        return new SimpleTextOutput($"bill {deletedBill.Name} deleted");
                    default:
                        throw new SubCommandNotSupportedException(ReservedWordEnum.DELETE, ReservedWordToken.ReservedWord);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(ReservedWordEnum.ADD);
            }
        }

        OutputTokenBase EvaluatePaidCommand(List<BudgetTokenBase> remainingTokens)
        {
            OneTimeBillModel model = EvaluateHelper.GetBillFromArgs(remainingTokens, Session);
            model.IsPaid = true;
            return new SimpleTextOutput($"Marked {model.Name} as paid");
        }

        OutputTokenBase EvaluateUnpaidCommand(List<BudgetTokenBase> remainingTokens)
        {
            OneTimeBillModel model = EvaluateHelper.GetBillFromArgs(remainingTokens, Session);
            model.IsPaid = false;
            return new SimpleTextOutput($"Marked {model.Name} as unpaid");
        }


        OutputTokenBase EvaluateSaveCommand()
        {
            Session?.SaveSession();
            return new SaveNotification();
        }

        OutputTokenBase EvaluateResetCommand()
        {
            Session?.ResetSession();
            return new ResetNotification();
        }

        void OnSafeExitEvent(EventArgs e)
        {
            SafeExitEvent?.Invoke(this, e);
        }

        void OnUnsavedChangesExitEvent(EventArgs e)
        {
            UnsavedChangesExitEvent?.Invoke(this, e);
        }
    }
}