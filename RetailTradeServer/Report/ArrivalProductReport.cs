using RetailTrade.Domain.Models;

namespace RetailTradeServer.Report
{
    public partial class ArrivalProductReport
    {
        public ArrivalProductReport(Arrival arrival)
        {
            InitializeComponent();

            lbSupplier.Text = $"Поставщик: {arrival.Supplier.ShortName}";
            lbTitle.Text = $"Приходная накладная № {string.Format("{0:D8}", arrival.Id)} от {string.Format("{0:dd.MM.yyyy}", arrival.ArrivalDate)}";
        }
    }
}
