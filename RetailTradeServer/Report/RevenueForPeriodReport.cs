using RetailTrade.Domain.Models;
using System;

namespace RetailTradeServer.Report
{
    public partial class RevenueForPeriodReport
    {
        public RevenueForPeriodReport(Organization organization, 
            DateTime startDate, 
            DateTime endDate)
        {
            InitializeComponent();

            lbOrganizationName.Text = organization.Name;
            lbAddress.Text = organization.Address;
            lbTitle.Text = $"������� �� ������ � {startDate.Date:dd.MM.yyyy)} �� {endDate.Date:dd.MM.yyyy}";
            //tcCash.Value = cash;
            //tcExpenditure.Value = expenditure;
        }
    }
}
