namespace BudgetCLI.Renderer.RenderObjects
{
    public class DataTable<DataType>
    {
        List<DataTableColumnInfo<DataType>> ColumnInfos;
        List<DataType> Data;
        char ColumnSeparator = '|';
        public DataTable()
        {
            ColumnInfos = [];
            Data = [];
        }

        public void AddColumn(string columnName, int columnWidth, Func<DataType, string> selector, TextAlignment textAlignment = TextAlignment.LEFT)
        {
            DataTableColumnInfo<DataType> newColumn = new(columnName, columnWidth, selector, textAlignment);
            ColumnInfos.Add(newColumn);
        }

        public void SetData(List<DataType> rowData)
        {
            Data = rowData;
        }

        public void AddData(DataType newDatum)
        {
            Data.Add(newDatum);
        }

        public int GetRowNumber(DataType datum)
        {
            return Data.IndexOf(datum);
        }

        public void Render()
        {
            RenderHeader();
            foreach (DataType datum in Data)
            {
                RenderRow(datum);
            }
        }

        void RenderHeader()
        {
            Console.Write(ColumnSeparator);
            foreach (DataTableColumnInfo<DataType> column in ColumnInfos)
            {
                column.RenderColumnHeader();
                Console.Write(ColumnSeparator);
            }
            Console.WriteLine();
            
            Console.Write('+');
            foreach (DataTableColumnInfo<DataType> column in ColumnInfos)
            {
                Console.Write(new string('-', column.ColumnWidth));
                Console.Write('+');
            }
            Console.WriteLine();
        }

        void RenderRow(DataType datum)
        {
            Console.Write(ColumnSeparator);
            foreach (DataTableColumnInfo<DataType> column in ColumnInfos)
            {
                column.RenderColumnDatum(datum);
                Console.Write(ColumnSeparator);
            }
            Console.WriteLine();
        }
    }

    class DataTableColumnInfo<DataType>
    {
        public string ColumnName;
        public int ColumnWidth;
        public Func<DataType, string> Selector;
        public TextAlignment TextAlignment;
        public DataTableColumnInfo(string columnName, int columnWidth, Func<DataType, string> selector, TextAlignment textAlignment = TextAlignment.LEFT)
        {
            ColumnName = columnName;
            ColumnWidth = columnWidth;
            Selector = selector;
            TextAlignment = textAlignment;
        }

        public void RenderColumnHeader()
        {
            Console.Write(RenderHelper.MakeColumnString(ColumnName, ColumnWidth, TextAlignment));
        }

        public void RenderColumnDatum(DataType datum)
        {
            string rawDatum = Selector(datum);
            Console.Write(RenderHelper.MakeColumnString(rawDatum, ColumnWidth, TextAlignment));
        }
    }
}