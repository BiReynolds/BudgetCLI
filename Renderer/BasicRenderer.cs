using BudgetCLI.Core.Interfaces;
using BudgetCLI.Core.Objects;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;

namespace BudgetCLI.Renderer
{
    public class BasicRenderer : IRenderer
    {
        public BasicRenderer()
        {

        }
        public void Render(OutputTokenBase outputToken)
        {
            switch (outputToken.OutputTokenType)
            {
                case OutputTokenEnum.SIMPLE_TEXT:
                    RenderSimpleText((SimpleTextOutput)outputToken);
                    return;
                case OutputTokenEnum.ERROR_TEXT:
                    RenderErrorText((ErrorTextOutput)outputToken);
                    return;
                case OutputTokenEnum.SINGLE_ONE_TIME_BILL:
                    RenderSingleOneTimeBill((SingleOneTimeBillModelDetail)outputToken);
                    return;
                default:
                    throw new OutputTokenNotSupportedException(outputToken);
            }
        }
        
        public void RenderSimpleText(SimpleTextOutput simpleTextOutput)
        {
            Console.WriteLine(simpleTextOutput.Content);
        }

        public void RenderErrorText(ErrorTextOutput errorTextOutput)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorTextOutput.ErrorInfo);
            Console.ResetColor();
        }

        public void RenderSingleOneTimeBill(SingleOneTimeBillModelDetail modelDetails)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (modelDetails.Id == -1)
            {
                Console.WriteLine("Id: Not yet assigned");
            }
            else
            {
                Console.WriteLine($"Id: {modelDetails.Id}");
            }
            Console.WriteLine($"Name : {modelDetails.Name}");
            Console.WriteLine($"Amount : {modelDetails.Amount}");
            Console.WriteLine($"Due Date : {modelDetails.DueDate}");
            Console.WriteLine($"Is Paid? : {modelDetails.IsPaid}");
            Console.ResetColor();
        }
    }
}