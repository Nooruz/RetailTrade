using System;

namespace RetailTradeServer.Report
{
    public partial class ClosingShiftsReport
    {
        public ClosingShiftsReport(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            lbDateRange.Text = $"� {startDate.ToString("dd.MM.yyyy")} �� {endDate.ToString("dd.MM.yyyy")}";
        }
    }
}
