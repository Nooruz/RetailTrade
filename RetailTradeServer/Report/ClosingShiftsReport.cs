using System;

namespace RetailTradeServer.Report
{
    public partial class ClosingShiftsReport
    {
        public ClosingShiftsReport(DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            lbDateRange.Text = $"с {startDate.ToString("dd.MM.yyyy")} по {endDate.ToString("dd.MM.yyyy")}";
        }
    }
}
