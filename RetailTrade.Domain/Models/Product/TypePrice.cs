namespace RetailTrade.Domain.Models
{
    public class TypePrice : DomainObject
    {
        public string Name { get; set; }
        public virtual PriceProduct PriceProduct { get; set; } 
    }
}
