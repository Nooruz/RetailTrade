using RetailTrade.Domain.Models;
using RetailTradeClient.State.Users;
using System;

namespace RetailTradeClient.Report
{
    public partial class ProductSaleReport
    {
        public ProductSaleReport(IUserStore userStore,
            Receipt receipt)
        {
            InitializeComponent();
            try
            {
                CashDate.Text = $"Дата: {receipt.DateOfPurchase.ToShortDateString()}";
                CashTime.Text = $"Время: {receipt.DateOfPurchase.ToShortTimeString()}";
                CashReceipt.Text = $"Товарный чек № {receipt.Id:D6}";
                lbUserFullName.Text = userStore.CurrentUser.FullName;
                lbCash.Text = $"{receipt.PaidInCash:N2} сом";
                lbCashless.Text = $"{receipt.PaidInCashless:N2} сом";
                lbChange.Text = $"{receipt.Change:N2} сом";
                if (userStore.Organization != null)
                {
                    OrganizationName.Text = userStore.Organization.Name;
                    Address.Text = userStore.Organization.Address;
                }
            }
            catch (Exception e)
            {
                //ignore
            }            
        }
    }
}
