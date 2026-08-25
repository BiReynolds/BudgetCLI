using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Scanner;
using BudgetCLI.Scanner.Tokens;

namespace BudgetCLI.Evaluator
{
    public class BasicEvaluator
    {
        public OutputTokenBase Evaluate(List<BudgetTokenBase> tokens)
        {
            BudgetTokenBase firstToken = tokens[0];
            if (firstToken.TokenType != BudgetTokenEnum.MAIN_COMMAND)
            {
                throw new Exception($"input should begin with a recognized command, instead received token {firstToken.RawToken} of type {firstToken.TokenType}");
            }
            MainCommandToken? commandToken = firstToken as MainCommandToken;
            if (commandToken == null)
            {
                throw new Exception($"Budget Token has TokenType of MAIN_COMMAND but could not be parsed as MainCommandToken");
            }
            switch (commandToken.CommandType)
            {
                case BudgetMainCommandEnum.SHOW:
                    return EvaluateShowCommand(tokens[1..]);
                default:
                    throw new Exception($"BasicEvaluator does not support command {commandToken.CommandType}");
            }
        }

        OutputTokenBase EvaluateShowCommand(List<BudgetTokenBase> remainingTokens)
        {
            return new SimpleTextOutput(remainingTokens);
        }
    }
}