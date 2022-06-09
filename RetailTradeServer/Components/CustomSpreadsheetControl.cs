using DevExpress.Xpf.Spreadsheet;

namespace RetailTradeServer.Components
{
    public class CustomSpreadsheetControl : SpreadsheetControl
    {
        public CustomSpreadsheetControl()
        {
            ActiveWorksheet.Cells.RowHeight = 50;
            ActiveWorksheet.Cells.ColumnWidth = 50;
            ActiveWorksheet.ActiveView.ShowGridlines = false;
        }
    }
}
