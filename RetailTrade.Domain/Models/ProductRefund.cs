namespace RetailTrade.Domain.Models
{
    public class ProductRefund : DomainObject
    {
        public int ProductId { get; set; }
        public int ShiftId { get; set; }

    }
}
