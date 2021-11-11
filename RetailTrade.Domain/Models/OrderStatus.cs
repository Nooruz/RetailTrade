using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class OrderStatus : DomainObject
    {
        public string Name { get; set; }
        public ICollection<OrderToSupplier> Orders { get; set; }
    }
}
