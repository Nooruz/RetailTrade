using RetailTrade.Domain.Models;

namespace SalePageServer.Report
{
    public partial class RevaluationReport
    {
        public RevaluationReport(Revaluation revaluation)
        {
            InitializeComponent();

            if (revaluation != null)
            {
                lblTitle.Text = $"��������� ��� ������� � {revaluation.Id} �� {revaluation.RevaluationDate:dd MMMM yyyy} ����";
            }
        }
    }
}
