using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class ProductRefund : DomainObject
    {
        #region Private Members

        private double _quantity;
        private decimal _sum;
        private decimal _purchasePrice;
        private decimal _retailPrice;
        private int _productId;
        private int _refundId;
        private int? _productSaleId;

        #endregion

        #region Public Properties

        /// <summary>
        /// Количество
        /// </summary>
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        /// <summary>
        /// Сумма
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged(nameof(Sum));
            }
        }

        /// <summary>
        /// Цена прихода
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged(nameof(PurchasePrice));
            }
        }

        /// <summary>
        /// Цена продажи
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailPrice
        {
            get => _retailPrice;
            set
            {
                _retailPrice = value;
                OnPropertyChanged(nameof(RetailPrice));
            }
        }

        /// <summary>
        /// Код товара
        /// </summary>
        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }

        /// <summary>
        /// Код чека
        /// </summary>
        public int RefundtId
        {
            get => _refundId;
            set
            {
                _refundId = value;
                OnPropertyChanged(nameof(RefundtId));
            }
        }

        public int? ProductSaleId
        {
            get => _productSaleId;
            set
            {
                _productSaleId = value;
                OnPropertyChanged(nameof(ProductSaleId));
            }
        }

        /// <summary>
        /// Чек возврат
        /// </summary>
        public Refund Refund { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }

        public ProductSale ProductSale { get; set; }

        #endregion
    }
}
