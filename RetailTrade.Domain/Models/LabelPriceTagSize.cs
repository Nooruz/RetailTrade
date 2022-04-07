using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class LabelPriceTagSize : DomainObject
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TypeLabelPriceTagId { get; set; }
        public TypeLabelPriceTag TypeLabelPriceTag { get; set; }
        public ICollection<LabelPriceTag> LabelPriceTags { get; set; }
    }
}
