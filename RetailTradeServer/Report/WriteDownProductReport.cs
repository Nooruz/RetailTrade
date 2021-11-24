using System;

namespace RetailTradeServer.Report
{
    public partial class WriteDownProductReport
    {
        public WriteDownProductReport(int id, DateTime date)
        {
            InitializeComponent();

            lbTitle.Text = string.Format("Списание товаров № {0:D8} от {1:dd.MM.yyyy}", id, date);
        }
    }
}
