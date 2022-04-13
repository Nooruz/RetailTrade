using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Gender : DomainObject
    {
        public string Name { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
