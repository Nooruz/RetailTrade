using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Role : DomainObject
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
