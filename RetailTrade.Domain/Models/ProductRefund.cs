using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class ProductRefund : DomainObject
    {
        /// <summary>
        /// Количество
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum { get; set; }

        /// <summary>
        /// Цена прихода
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrivalPrice { get; set; }

        /// <summary>
        /// Цена продажи
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
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
