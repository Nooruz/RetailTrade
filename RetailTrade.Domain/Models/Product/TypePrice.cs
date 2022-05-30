namespace RetailTrade.Domain.Models
{
    public class TypePrice : DomainObject
    {
        public string Name { get; set; }
        public int PriceProductId { get; set; }
        public PriceProduct PriceProduct { get; set; } 
    }
}
