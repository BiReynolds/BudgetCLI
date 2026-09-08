using BudgetCLI.Core.Objects;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Session;
using BudgetCLI.Jobs;
using BudgetCLI.Wizards;

namespace BudgetCLI.Core
{
    public class BudgetEngine
    {
        public bool Running = false;
        IScanner Scanner;
        IEvaluator Evaluator;
        IRenderer Renderer;
        StartupWizard StartupWizard = new();
        string CurrentInput = "";
        List<BudgetTokenBase> CurrentScannedTokens = [];
        SessionManager Session;
        JobManager JobManager;
        OutputTokenBase? CurrentOutput;
        public BudgetEngine(IScanner scanner, IEvaluator evaluator, IRenderer renderer)
        {
            Scanner = scanner;
            Evaluator = evaluator;
            Renderer = renderer;
            Session = new();
            JobManager = new(Session);
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
            HandleStringInput(CurrentInput);
        }

        void HandleStringInput(string stringInput)
        {
            try
            {
                CurrentScannedTokens = Scanner.Scan(stringInput);
                CurrentOutput = Evaluator.Evaluate(CurrentScannedTokens);
            }
            catch (Exception e)
            {
                CurrentOutput = new ErrorToken(e);
            }
            Renderer.Render(CurrentOutput);
        }

        void HandleTokenInput(List<BudgetTokenBase> tokens)
        {
            CurrentInput = "";
            CurrentScannedTokens = tokens;
            try
            {
                CurrentOutput = Evaluator.Evaluate(CurrentScannedTokens);
            }
            catch (Exception e) 
            {
                CurrentOutput = new ErrorToken(e);
            }
            Renderer.Render(CurrentOutput);
        }

        public virtual void HandleEmptyInput()
        {
            Console.WriteLine("no input provided");
        }

        public virtual void InitActions()
        {
            // Console.WriteLine("Initializing Session");
            Session.InitSession();
            // Console.WriteLine("Handling Jobs");
            JobManager.HandleJobs();

            // Console.WriteLine("Setting session data");
            Evaluator.SetSessionData(Session);
            Evaluator.SafeExitEvent += (o, e) =>
            {
                Running = false;
            };

            List<List<BudgetTokenBase>> startupCommands = StartupWizard.GetCommandsFromWizard();
            foreach (var commandTokens in startupCommands)
            {
                HandleTokenInput(commandTokens);
            }
            Console.WriteLine("Budget program started");
        }

        public virtual void ExitActions()
        {
            Console.WriteLine("Budget program closed successfully");
        }
    }
}