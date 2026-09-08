using BudgetCLI.Core.Objects;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Data.Models;
using BudgetCLI.Session;
using BudgetCLI.Evaluator.SpecialEvaluators;
using BudgetCLI.Wizards;

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
            if (Session == null || ShowCommandEvaluator == null || EditCommandEvaluator == null)
            {
                throw new Exception("Evaluator data is null at Evaluate - did you call SetSessionData?");
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
                    case ReservedWordEnum.PROJECTION:
                        return EvaluateProjectionCommand(tokens[1..]);
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
            int[] validNumCommands = [1, 4, 5, 6];
            if (!validNumCommands.Contains(remainingTokens.Count))
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, validNumCommands);
            }
            if (remainingTokens.Count != 1)
            {
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
            }
            if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                ReservedWordToken ReservedWordToken = (ReservedWordToken)remainingTokens[0];
                switch (ReservedWordToken.ReservedWord)
                {
                    case ReservedWordEnum.BILL:
                        if (remainingTokens.Count == 1)
                        {
                            AddBillWizard wizard = new();
                            List<List<BudgetTokenBase>> addBillWizardCommands = wizard.GetCommandsFromWizard();
                            if (addBillWizardCommands.Count == 0)
                            {
                                return new SimpleTextOutput("Add Bill Wizard was cancelled");
                            }
                            else
                            {
                                return Evaluate(addBillWizardCommands[0]);
                            }
                        }
                        else if (remainingTokens.Count == 4)
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
                            Session.SaveSession();
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
                ReservedWordToken ReservedWordToken = (ReservedWordToken)remainingTokens[0];
                switch (ReservedWordToken.ReservedWord)
                {
                    case ReservedWordEnum.BILL:
                        OneTimeBillModel deletedBill = EvaluateHelper.GetBillFromArgs(remainingTokens[1..], Session);
                        Session.DeleteOneTimeBill(deletedBill);
                        return new SimpleTextOutput($"bill {deletedBill.Name} deleted");
                    case ReservedWordEnum.RECURRING:
                        RecurringBillModel deletedRecurringBill = EvaluateHelper.GetRecurringBillFromArgs(remainingTokens[1..], Session);
                        Session.DeleteRecurringBillAndUnpaidInstances(deletedRecurringBill);
                        return new SimpleTextOutput($"recurring bill {deletedRecurringBill.Name} deleted");
                    default:
                        throw new SubCommandNotSupportedException(ReservedWordEnum.DELETE, ReservedWordToken.ReservedWord);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(ReservedWordEnum.DELETE);
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

        OutputTokenBase EvaluateProjectionCommand(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens.Count == 0)
            {
                return EvaluateHelper.GetProjectionTable(Session.SessionBillList, Session.SessionBalance, 1);
            }
            else if (remainingTokens.Count > 1)
            {
                throw new WrongNumberOfArgumentsException(remainingTokens.Count, 1);
            }
            else if (remainingTokens[0].TokenType == BudgetTokenEnum.NUMBER)
            {
                var numMonthsToken = (NumberToken)remainingTokens[0];
                return EvaluateHelper.GetProjectionTable(Session.SessionBillList, Session.SessionBalance, (int)numMonthsToken.Value);
            }
            else if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                var subCommandToken = (ReservedWordToken)remainingTokens[0];
                if (subCommandToken.ReservedWord != ReservedWordEnum.SUMMARY)
                {
                    throw new SubCommandNotSupportedException(ReservedWordEnum.PROJECTION, subCommandToken.ReservedWord);
                }
                else 
                {
                    ProjectionTableData projectionData = EvaluateHelper.GetProjectionTable(Session.SessionBillList, Session.SessionBalance, 3);
                    ProjectionTableDataRow? nextThirtyMinRow = projectionData.Rows.Where(x => x.Date < Session.Today.AddMonths(1)).MinBy(x => x.Balance);
                    ProjectionTableDataRow? thirtyToSixtyMinRow = projectionData.Rows.Where(x => x.Date >= Session.Today.AddMonths(1) && x.Date < Session.Today.AddMonths(2)).MinBy(x => x.Balance);
                    ProjectionTableDataRow? sixtyToNinetyMinRow = projectionData.Rows.Where(x => x.Date >= Session.Today.AddMonths(2) && x.Date < Session.Today.AddMonths(3)).MinBy(x => x.Balance);
                    if (nextThirtyMinRow == null || thirtyToSixtyMinRow == null || sixtyToNinetyMinRow == null)
                    {
                        throw new Exception("problem finding one or more rows for projection summary...");
                    }
                    return new ProjectionSummary(nextThirtyMinRow, thirtyToSixtyMinRow, sixtyToNinetyMinRow);
                }
            }
            else
            {
                throw new UnexpectedArgTypeException(remainingTokens[0], [BudgetTokenEnum.NUMBER, BudgetTokenEnum.RESERVED_WORD]);
            }
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