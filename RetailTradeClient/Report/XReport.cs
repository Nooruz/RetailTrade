using RetailTrade.Domain.Models;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RetailTradeClient.Report
{
    public partial class XReport
    {
        private readonly IUserStore _userStore;
        private readonly IShiftStore _shiftStore;
        public XReport(IUserStore userStore,
            IShiftStore shiftStore)
        {
            InitializeComponent();
            _userStore = userStore;
            _shiftStore = shiftStore;
        }

        public void SetValues()
        {
            try
            {
                ObservableCollection<Receipt> receipts = _shiftStore.GetReceipts();

                OrganizationName.Text = _userStore.Organization.ShortName;
                Address.Text = _userStore.Organization.Address;
                lbCashierName.Text = _userStore.CurrentUser.FullName;
                lbReportDate.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                lbShiftNumber.Text = $"����� � {_shiftStore.CurrentShift.Id:0000}";
                lbStartShift.Text = $"������ �����: {_shiftStore.CurrentShift.OpeningDate:dd.MM.yyyy HH:mm}";
                lbCashSum.Text = $"{receipts?.Sum(r => r.PaidInCash)} ���";
                lbCashlessSum.Text = $"{receipts?.Sum(r => r.PaidInCashless)} ���";
                lbReceiptCount.Text = $"#{receipts?.Count:00000}";
                lbCashOnHand.Text = $"{receipts?.Sum(r => r.Total)} ���";
                lbShiftTotal.Text = $"{receipts?.Sum(r => r.Total)} ���";
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}
