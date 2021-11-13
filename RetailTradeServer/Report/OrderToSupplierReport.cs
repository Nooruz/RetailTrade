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
                lbOrderNumberAndDate.Text = $"Заказ поставшику № {orderToSupplier.Id} от {orderToSupplier.OrderDate.ToShortDateString()}";
                lbSupplier.Text = $"{supplier.FullName} ИНН {supplier.Inn}, {supplier.Address}, тел.: {supplier.Phone}";
                lbBuyer.Text = $"{organization.FullName}, ИНН {organization.Inn}, {organization.Address}, тел.: {organization.Phone}";
            }
            catch (Exception e)
            {
                //ignore
            }
        }
    }
}
