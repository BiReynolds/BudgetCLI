using BudgetCLI.Core.Interfaces;
using BudgetCLI.Core.Objects;
using BudgetCLI.Evaluator.OutputTokens;
using BudgetCLI.Exceptions;
using BudgetCLI.Renderer.RenderObjects;

namespace BudgetCLI.Renderer
{
    public class BasicRenderer : IRenderer
    {
        public BasicRenderer()
        {
        }

        public void Render(OutputTokenBase outputToken)
        {
            RenderHelper.SetColorsToDefaultResponse();
            switch (outputToken.OutputTokenType)
            {
                case OutputTokenEnum.EXIT_NOTIFICATION:
                    RenderExitNotification((ExitNotification)outputToken);
                    break;
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
                case OutputTokenEnum.SAVE_NOTIFICATION:
                    RenderSaveNotification((SaveNotification)outputToken);
                    break;
                case OutputTokenEnum.RESET_NOTIFICATION:
                    RenderResetNotification((ResetNotification)outputToken);
                    break;
                case OutputTokenEnum.RECURRING_BILL_LIST:
                    RenderRecurringBillList((RecurringBillList)outputToken);
                    break;
                case OutputTokenEnum.PROJECTION_TABLE:
                    RenderProjectionTable((ProjectionTableData)outputToken);
                    break;
                case OutputTokenEnum.PROJECTION_SUMMARY:
                    RenderProjectionSummary((ProjectionSummary)outputToken);
                    break;
                default:
                    throw new OutputTokenNotSupportedException(outputToken);
            }
            RenderHelper.SetColorsToDefaultInput();
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
            if (modelDetails.ParentId != null)
            {
                Console.WriteLine($"ParentId : {modelDetails.ParentId}");
            }
            Console.WriteLine($"Name : {modelDetails.Name}");
            Console.WriteLine($"Amount : {modelDetails.Amount}");
            Console.WriteLine($"Due Date : {modelDetails.DueDate}");
            Console.WriteLine($"Is Paid? : {modelDetails.IsPaid}");
        }

        public void RenderOneTimeBillList(OneTimeBillList billList)
        {
            DataTable<SingleOneTimeBillModelDetail> dataTable = new();
            dataTable.AddColumn("Id", 3, x => RenderHelper.GetIdOrQuestionMark(x.Id));
            dataTable.AddColumn("Name", 25, x => x.Name);
            dataTable.AddColumn("Amount", 13, x => x.Amount.ToString("C"), TextAlignment.RIGHT);
            dataTable.AddColumn("Due Date", 12, x => x.DueDate.ToShortDateString(), TextAlignment.CENTER);
            dataTable.AddColumn("Paid?", 5, x => { return x.IsPaid ? "x" : ""; }, TextAlignment.CENTER);
            
            dataTable.SetData(billList.Data);
            dataTable.Render();
        }

        public void RenderRecurringBillList(RecurringBillList recurringBillList)
        {
            DataTable<RecurringBillDetail> dataTable = new();
            dataTable.AddColumn("Id", 3, x => RenderHelper.GetIdOrQuestionMark(x.Id));
            dataTable.AddColumn("Name", 25, x => x.Name);
            dataTable.AddColumn("Amount", 13, x => x.Amount.ToString("C"), TextAlignment.RIGHT);
            dataTable.AddColumn("Start Date", 12, x => x.StartDate.ToShortDateString(), TextAlignment.CENTER);
            dataTable.AddColumn("End Date", 12, x => x.EndDate?.ToShortDateString() ?? "NONE", TextAlignment.CENTER);
            dataTable.AddColumn("Recurring Type", 15, x => x.RecurringType.ToString());
            dataTable.AddColumn("Reference Date", 12, x => x.ReferenceDate.ToString(), TextAlignment.CENTER);

            dataTable.SetData(recurringBillList.Data);
            dataTable.Render();
        }

        public void RenderProjectionTable(ProjectionTableData data)
        {
            Console.WriteLine($"Starting Balance: {data.StartBalance}");
            Console.WriteLine($"Bills still due: {string.Join(", ", data.StillDueBills)}");
            Console.WriteLine($"Adjusted Starting Balance: {data.AdjStartBalance}");

            DataTable<ProjectionTableDataRow> dataTable = new();
            dataTable.AddColumn("Date", 12, x => x.Date.ToShortDateString(), TextAlignment.CENTER);
            dataTable.AddColumn("Balance", 13, x => x.Balance.ToString(), TextAlignment.RIGHT);
            dataTable.AddColumn("Bills Due", 50, x => string.Join(", ", x.BillsDue));

            dataTable.SetData(data.Rows);
            dataTable.Render();
        }

        public void RenderProjectionSummary(ProjectionSummary summary)
        {
            string[] rowLabels = ["Next 30 Days", "30 - 60 Days", "60 - 90 Days"];
            DataTable<ProjectionTableDataRow> dataTable = new();
            dataTable.AddColumn("Time Range", 15, x => rowLabels[dataTable.GetRowNumber(x)]);
            dataTable.AddColumn("Minimum Balance Date", 20, x => x.Date.ToShortDateString(), TextAlignment.CENTER);
            dataTable.AddColumn("Minimum Balance", 15, x => x.Balance.ToString(), TextAlignment.RIGHT);
            dataTable.AddColumn("Bills Due", 50, x => string.Join(", ", x.BillsDue));

            dataTable.SetData([summary.NextThirtyMinRow, summary.ThirtyToSixtyMinRow, summary.SixtyToNinetyMinRow]);
            dataTable.Render();
        }

        public void RenderExitNotification(ExitNotification exitNotification)
        {
            if (exitNotification.ExitSuccess)
            {
                Console.WriteLine(exitNotification.SuccessfulExitText);
            }
            else
            {
                Console.WriteLine(exitNotification.UnsuccessfulExitText);
            }
        }

        public void RenderSaveNotification(SaveNotification saveNotification)
        {
            Console.WriteLine(saveNotification.SaveText);
        }

        public void RenderResetNotification(ResetNotification resetNotification)
        {
            Console.WriteLine(resetNotification.ResetText);
        }


    }
}