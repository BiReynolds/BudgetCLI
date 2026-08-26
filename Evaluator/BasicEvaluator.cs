using BudgetCLI.Core.Objects;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Scanner.Tokens;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;

namespace BudgetCLI.Evaluator 
{
    public class BasicEvaluator : IEvaluator
    {
        public OutputTokenBase Evaluate(List<BudgetTokenBase> tokens)
        {
            BudgetTokenBase firstToken = tokens[0];
            if (firstToken.TokenType != BudgetTokenEnum.MAIN_COMMAND)
            {
                throw new NoLeadingCommandException(firstToken);
            }
            MainCommandToken? commandToken = firstToken as MainCommandToken;
            if (commandToken == null)
            {
                throw new TokenTypeMismatchException(BudgetTokenEnum.MAIN_COMMAND, typeof(MainCommandToken));
            }
            switch (commandToken.CommandType)
            {
                case BudgetMainCommandEnum.SHOW:
                    return EvaluateShowCommand(tokens[1..]);
                default:
                    throw new CommandNotSupportedException(commandToken);
            }
        }

        OutputTokenBase EvaluateShowCommand(List<BudgetTokenBase> remainingTokens)
        {
            return new SimpleTextOutput(remainingTokens);
        }
    }
}