using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class ContractorType : DomainObject
    {
        public string Name { get; set; }
        public ICollection<Contractor> Contractors { get; set; }
    }
}
