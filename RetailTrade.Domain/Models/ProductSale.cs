namespace RetailTrade.Domain.Models
{
    public class ProductSale : DomainObject
    {
        /// <summary>
        /// Количество
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Код товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Код чека
        /// </summary>
        public int ReceiptId { get; set; }

        /// <summary>
        /// Чек
        /// </summary>
        public Receipt Receipt { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }
    }
}
