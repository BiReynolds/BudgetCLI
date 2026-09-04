using BudgetCLI.Core.Objects;

namespace BudgetCLI.Evaluator.OutputTokens
{
    public class ProjectionTableData : OutputTokenBase
    {
        public decimal StartBalance;
        public decimal AdjStartBalance;
        public string[] StillDueBills;
        public List<ProjectionTableDataRow> Rows = new();
        public ProjectionTableData(decimal startBalance, decimal adjStartBalance, IEnumerable<string> stillDueBills) : base(OutputTokenEnum.PROJECTION_TABLE)
        {
            StartBalance = startBalance;
            AdjStartBalance = adjStartBalance;
            StillDueBills = stillDueBills.ToArray();
        }
        public void AddRow(DateOnly date, decimal balance, IEnumerable<string> billsDue)
        {
            Rows.Add(new ProjectionTableDataRow(date, balance, billsDue));
        }
    }

    public class ProjectionTableDataRow
    {
        public DateOnly Date;
        public decimal Balance;
        public string[] BillsDue;
        public ProjectionTableDataRow(DateOnly date, decimal balance, IEnumerable<string> billsDue)
        {
            Date = date;
            Balance = balance;
            BillsDue = billsDue.ToArray();
        }
    }
}