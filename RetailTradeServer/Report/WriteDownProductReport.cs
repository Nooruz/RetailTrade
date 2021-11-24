using System;

namespace RetailTradeServer.Report
{
    public partial class WriteDownProductReport
    {
        public WriteDownProductReport(int id, DateTime date)
        {
            InitializeComponent();

            lbTitle.Text = string.Format("�������� ������� � {0:D8} �� {1:dd.MM.yyyy}", id, date);
        }
    }
}
