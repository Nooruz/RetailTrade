using RetailTrade.Domain.Models;
using RetailTradeClient.State.Users;

namespace RetailTradeClient.Report
{
    public partial class ProductSaleReport
    {
        public ProductSaleReport(IUserStore userStore,
            Receipt receipt)
        {
            InitializeComponent();
            OrganizationName.Text = userStore.Organization.Name;
            Address.Text = userStore.Organization.Address;
            CashDate.Text = $"Дата: {receipt.DateOfPurchase.ToShortDateString()}";
            CashTime.Text = $"Время: {receipt.DateOfPurchase.ToShortTimeString()}";
            CashReceipt.Text = $"Товарный чек № {receipt.Id.ToString("D6")}";
            lbUserFullName.Text = userStore.CurrentUser.FullName;
        }
    }
}
