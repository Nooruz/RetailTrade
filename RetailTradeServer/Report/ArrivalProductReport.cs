using RetailTrade.Domain.Models;

namespace RetailTradeServer.Report
{
    public partial class ArrivalProductReport
    {
        public ArrivalProductReport(Arrival arrival)
        {
            InitializeComponent();

            lbSupplier.Text = $"���������: {arrival.Supplier.ShortName}";
            lbTitle.Text = $"��������� ��������� � {string.Format("{0:D8}", arrival.Id)} �� {string.Format("{0:dd.MM.yyyy}", arrival.ArrivalDate)}";
        }
    }
}
