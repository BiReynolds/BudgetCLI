using BudgetCLI.Core;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Evaluator;
using BudgetCLI.Renderer;
using BudgetCLI.Scanner;

namespace BudgetCLI
{
    public static class Program
    {
        static IScanner Scanner = new BasicCommandScanner();
        static IEvaluator Evaluator = new BasicEvaluator();
        static IRenderer Renderer = new BasicRenderer();
        public static void Main()
        {
            BudgetEngine engine = new(Scanner, Evaluator, Renderer);
            engine.Start();
        }
    }
}