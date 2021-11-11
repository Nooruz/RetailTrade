using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class OrderProduct : DomainObject
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public Product Product { get; set; }
        public ICollection<OrderToSupplier> OrderToSuppliers { get; set; }
    }
}
