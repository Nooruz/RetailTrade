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
                CashDate.Text = $"����: {receipt.DateOfPurchase.ToShortDateString()}";
                CashTime.Text = $"�����: {receipt.DateOfPurchase.ToShortTimeString()}";
                CashReceipt.Text = $"�������� ��� � {receipt.Id.ToString("D6")}";
                lbUserFullName.Text = userStore.CurrentUser.FullName;
                OrganizationName.Text = userStore.Organization.Name;
                Address.Text = userStore.Organization.Address;
            }
            catch (Exception e)
            {
                //ignore
            }            
        }
    }
}
