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
                lblTitle.Text = $"Установка цен товаров № {revaluation.Id} от {revaluation.RevaluationDate:dd MMMM yyyy} года";
            }
        }
    }
}
