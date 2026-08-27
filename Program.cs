using BudgetCLI.Core;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Evaluator;
using BudgetCLI.Renderer;
using BudgetCLI.Scanner;
using BudgetCLI.Data;
using BudgetCLI.Data.Models;
using BudgetCLI.Testing;

namespace BudgetCLI
{
    public static class Program
    {
        static IScanner Scanner = new BasicCommandScanner();
        static IEvaluator Evaluator = new BasicEvaluator();
        static IRenderer Renderer = new BasicRenderer();
        static MigrationManager MigrationManager = new();
        public static void Main()
        {
            MigrationManager.DoMigrations();
            RunTests();
            BudgetEngine engine = new(Scanner, Evaluator, Renderer);
        }

        public static void RunTests()
        {
            DatabaseTesting.OneTimeBillTest();
        }
    }
}