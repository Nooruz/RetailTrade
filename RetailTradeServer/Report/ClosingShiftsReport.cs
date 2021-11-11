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
                lbDateRange.Text = $"� {startDate.ToString("dd.MM.yyyy")} �� {endDate.ToString("dd.MM.yyyy")}";
            }            
        }
    }
}
