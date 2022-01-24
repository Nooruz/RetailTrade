using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class TypeWareHouse : DomainObject
    {
        public string Name { get; set; }
        public ICollection<WareHouse> WareHouses { get; set; }
    }
}
