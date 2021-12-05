namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Отложенные товары
    /// </summary>
    public class PostponeProduct
    {
        public int Id { get; set; }
        public int PostponeReceiptId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public decimal ArrivalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Sum { get; set; }

        public PostponeReceipt PostponeReceipt { get; set; }
    }
}
