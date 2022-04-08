using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class TypeLabelPriceTag : DomainObject
    {

        public string Name { get; set; }
        public ICollection<LabelPriceTag> LabelPriceTags { get; set; }
        public ICollection<LabelPriceTagSize> LabelPriceTagSizes { get; set; }
    }
}
