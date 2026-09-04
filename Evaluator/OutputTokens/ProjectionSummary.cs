using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class ProjectionSummary : OutputTokenBase
    {
        public ProjectionTableDataRow NextThirtyMinRow;
        public ProjectionTableDataRow ThirtyToSixtyMinRow;
        public ProjectionTableDataRow SixtyToNinetyMinRow;
        public ProjectionSummary(ProjectionTableDataRow nextThirtyMinRow, ProjectionTableDataRow thirtyToSixtyMinRow, ProjectionTableDataRow sixtyToNinetyMinRow) : base(OutputTokenEnum.PROJECTION_SUMMARY)
        {
            NextThirtyMinRow = nextThirtyMinRow;
            ThirtyToSixtyMinRow = thirtyToSixtyMinRow;
            SixtyToNinetyMinRow = sixtyToNinetyMinRow;
        }
    }
}