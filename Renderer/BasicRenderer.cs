using System.Runtime;
using BudgetCLI.Core.Interfaces;
using BudgetCLI.Core.Objects;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Renderer.RenderObjects;

namespace BudgetCLI.Renderer
{
    public class BasicRenderer : IRenderer
    {
        ConsoleColor DefaultResponseTextColor, DefaultInputTextColor;
        public BasicRenderer(ConsoleColor defaultResponseTextColor = ConsoleColor.Blue, ConsoleColor defaultInputTextColor = ConsoleColor.White)
        {
            DefaultResponseTextColor = defaultResponseTextColor;
            DefaultInputTextColor = defaultInputTextColor;
        }

        public void Render(OutputTokenBase outputToken)
        {
            SetColorsToDefaultResponse();
            switch (outputToken.OutputTokenType)
            {
                case OutputTokenEnum.SIMPLE_TEXT:
                    RenderSimpleText((SimpleTextOutput)outputToken);
                    break;
                case OutputTokenEnum.ERROR:
                    RenderError((ErrorToken)outputToken);
                    break;
                case OutputTokenEnum.SINGLE_ONE_TIME_BILL:
                    RenderSingleOneTimeBill((SingleOneTimeBillModelDetail)outputToken);
                    break;
                case OutputTokenEnum.ONE_TIME_BILL_LIST:
                    RenderOneTimeBillList((OneTimeBillList)outputToken);
                    break;
                default:
                    throw new OutputTokenNotSupportedException(outputToken);
            }
            SetColorsToDefaultInput();
        }
        
        public void RenderSimpleText(SimpleTextOutput simpleTextOutput)
        {

            Console.WriteLine(simpleTextOutput.Content);
        }

        public void RenderError(ErrorToken errorTextOutput)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(errorTextOutput.EncounteredException);
        }

        public void RenderSingleOneTimeBill(SingleOneTimeBillModelDetail modelDetails)
        {
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
        }

        public void RenderOneTimeBillList(OneTimeBillList billList)
        {
            DataTable<SingleOneTimeBillModelDetail> dataTable = new();
            dataTable.AddColumn("Id", 3, x => x.Id.ToString());
            dataTable.AddColumn("Name", 15, x => x.Name);
            dataTable.AddColumn("Amount", 8, x => x.Amount.ToString("C"), TextAlignment.RIGHT);
            dataTable.AddColumn("Due Date", 12, x => x.DueDate.ToShortDateString(), TextAlignment.CENTER);
            dataTable.AddColumn("Paid?", 5, x => { return x.IsPaid ? "x" : ""; });
            
            dataTable.SetData(billList.Data);
            dataTable.Render();
        }

        void SetColorsToDefaultResponse()
        {
            Console.ForegroundColor = DefaultResponseTextColor;
        }

        void SetColorsToDefaultInput()
        {
            Console.ForegroundColor = DefaultInputTextColor;
        }
    }
}