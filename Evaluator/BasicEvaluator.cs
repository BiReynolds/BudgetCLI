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
            if (firstToken.TokenType != BudgetTokenEnum.RESERVED_WORD)
            {
                throw new NoLeadingCommandException(firstToken);
            }
            if (firstToken is not ReservedWordToken commandToken)
            {
                throw new TokenTypeMismatchException(BudgetTokenEnum.RESERVED_WORD, typeof(ReservedWordToken));
            }
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

        OutputTokenBase EvaluateAddCommand(List<BudgetTokenBase> remainingTokens)
        {
            if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                ReservedWordToken ReservedWordToken = remainingTokens[0] as ReservedWordToken;
                switch (ReservedWordToken.ReservedWord)
                {
                    case ReservedWordEnum.BILL:
                        GetAddBillArgs(remainingTokens[1..], out string name, out decimal amount, out DateOnly dueDate);
                        OneTimeBillModel model = new(name, amount, dueDate, false);
                        Session.AddNewOneTimeBill(model);
                        return new SimpleTextOutput($"Bill {name} added");
                    default:
                        throw new SubCommandNotSupportedException(ReservedWordEnum.ADD, ReservedWordToken.ReservedWord);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(ReservedWordEnum.ADD);
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
            if (remainingTokens[0].TokenType == BudgetTokenEnum.RESERVED_WORD)
            {
                ReservedWordToken ReservedWordToken = remainingTokens[0] as ReservedWordToken;
                switch (ReservedWordToken.ReservedWord)
                {
                    case ReservedWordEnum.BILL:
                        GetDeleteBillArgs(remainingTokens[1..], out int billId);
                        bool didDelete = Session.DeleteOneTimeBillById(billId);
                        if (didDelete)
                        {
                            return new SimpleTextOutput($"bill with id {billId} deleted");
                        }
                        else 
                        {
                            return new SimpleTextOutput($"no change made - no bill found with id {billId}");
                        }
                    default:
                        throw new SubCommandNotSupportedException(ReservedWordEnum.DELETE, ReservedWordToken.ReservedWord);
                }
            }
            else
            {
                throw new ExpectedSubCommandException(ReservedWordEnum.ADD);
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