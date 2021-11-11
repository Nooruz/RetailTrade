using System;

namespace RetailTradeServer.Report
{
    public partial class ClosingShiftsReport
    {
        public ClosingShiftsReport()
        {
            InitializeComponent();

            if (StartDate.Value is DateTime startDate && EndDate.Value is DateTime endDate)
            {
                lbDateRange.Text = $"с {startDate.ToString("dd.MM.yyyy")} по {endDate.ToString("dd.MM.yyyy")}";
            }            
        }
    }
}
