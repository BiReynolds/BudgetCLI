using BudgetCLI.Core.Objects;
using BudgetCLI.Core.Interfaces;

namespace BudgetCLI.Core
{
    public class BudgetEngine
    {
        public bool Running = false;
        IScanner Scanner;
        IEvaluator Evaluator;
        IRenderer Renderer;
        string CurrentInput = "";
        List<BudgetTokenBase> CurrentScannedTokens = [];
        OutputTokenBase? CurrentOutput;
        public BudgetEngine(IScanner scanner, IEvaluator evaluator, IRenderer renderer)
        {
            Scanner = scanner;
            Evaluator = evaluator;
            Renderer = renderer;
        }

        public void Start()
        {
            InitActions();
            Running = true;
            while (Running)
            {
                string rawInput = Console.ReadLine() ?? "";
                if (rawInput.Length == 0)
                {
                    HandleEmptyInput();
                }
                else if (rawInput.ToLower() == "exit")
                {
                    Running = false;
                }
                else
                {
                    CurrentInput = rawInput;
                    HandleUserInput();
                }
            }
            ExitActions();
        }

        public void HandleUserInput()
        {
            CurrentScannedTokens = Scanner.Scan(CurrentInput);
            CurrentOutput = Evaluator.Evaluate(CurrentScannedTokens);
            Renderer.Render(CurrentOutput);
        }

        public virtual void HandleEmptyInput()
        {
            Console.WriteLine("no input provided");
        }

        public virtual void InitActions()
        {
            Console.WriteLine("starting program");
        }

        public virtual void ExitActions()
        {
            Console.WriteLine("stopping program");
        }
    }
}