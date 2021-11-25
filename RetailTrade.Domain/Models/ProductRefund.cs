namespace RetailTrade.Domain.Models
{
    public class ProductRefund : DomainObject
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
        /// Цена продажи
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// Код товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Код чека
        /// </summary>
        public int RefundtId { get; set; }

        /// <summary>
        /// Чек возврат
        /// </summary>
        public Refund Refund { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }
    }
}
