using RetailTrade.Domain.Models;
using RetailTradeClient.State.Users;
using System;

namespace RetailTradeClient.Report
{
    public partial class ProductSaleReport
    {
        #region Private Members

        private readonly IUserStore _userStore;

        #endregion

        #region Constructor

        public ProductSaleReport(IUserStore userStore)
        {
            InitializeComponent();
            _userStore = userStore;
        }

        #endregion

        #region Public Voids

        public void SetValues(Receipt receipt)
        {
            try
            {
                CashDate.Text = $"����: {receipt.DateOfPurchase.ToShortDateString()}";
                CashTime.Text = $"�����: {receipt.DateOfPurchase.ToShortTimeString()}";
                CashReceipt.Text = $"�������� ��� � {receipt.Id:D6}";
                lbUserFullName.Text = _userStore.CurrentUser.FullName;
                lbCash.Text = $"{receipt.PaidInCash:N2} ���";
                lbCashless.Text = $"{receipt.PaidInCashless:N2} ���";
                lbChange.Text = $"{receipt.Change:N2} ���";
                if (_userStore.Organization != null)
                {
                    OrganizationName.Text = _userStore.Organization.Name;
                    Address.Text = _userStore.Organization.Address;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion
    }
}
