using RetailTrade.Domain.Models;
using RetailTradeClient.State.Users;
using System;
using System.Collections.Generic;

namespace RetailTradeClient.Report
{
    public partial class DiscountReceiptReport
    {
        #region Private Members

        private readonly IUserStore _userStore;

        #endregion

        #region Constructor

        public DiscountReceiptReport(IUserStore userStore)
        {
            InitializeComponent();
            _userStore = userStore;
        }

        #endregion        

        #region Public Voids

        public void SetValues(Receipt receipt, IEnumerable<Sale> sales)
        {
            try
            {
                DataSource = sales;
                CashDate.Text = $"����: {receipt.DateOfPurchase.ToShortDateString()}";
                CashTime.Text = $"�����: {receipt.DateOfPurchase.ToShortTimeString()}";
                CashReceipt.Text = $"�������� ��� � {receipt.Id:D6}";
                lbUserFullName.Text = _userStore.CurrentUser.FullName;
                lbToBePaid.Text = $"{receipt.PaidInCash + receipt.PaidInCashless:N2} ���";
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
