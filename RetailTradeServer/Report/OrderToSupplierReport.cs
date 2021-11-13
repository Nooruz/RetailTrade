using RetailTrade.Domain.Models;
using System;

namespace RetailTradeServer.Report
{
    public partial class OrderToSupplierReport
    {
        public OrderToSupplierReport(OrderToSupplier orderToSupplier, Supplier supplier, Organization organization)
        {
            InitializeComponent();
            try
            {
                lbOrderNumberAndDate.Text = $"����� ���������� � {orderToSupplier.Id} �� {orderToSupplier.OrderDate.ToShortDateString()}";
                lbSupplier.Text = $"{supplier.FullName} ��� {supplier.Inn}, {supplier.Address}, ���.: {supplier.Phone}";
                lbBuyer.Text = $"{organization.FullName}, ��� {organization.Inn}, {organization.Address}, ���.: {organization.Phone}";
            }
            catch (Exception e)
            {
                //ignore
            }
        }
    }
}
