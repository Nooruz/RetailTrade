using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Переоценка товаров
    /// </summary>
    public class RevaluationProduct : DomainObject
    {
        #region Private Members

        private int _productId;
        private int _revaluationId;
        private decimal _arrivalPrice;
        private decimal _salePrice;

        #endregion

        #region Public Properties

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
        /// Код историй изменения товара
        /// </summary>
        public int RevaluationId
        {
            get => _revaluationId;
            set
            {
                _revaluationId = value;
                OnPropertyChanged(nameof(RevaluationId));
            }
        }

        /// <summary>
        /// Цена прихода
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrivalPrice
        {
            get => _arrivalPrice;
            set
            {
                _arrivalPrice = value;
                OnPropertyChanged(nameof(ArrivalPrice));
            }
        }

        /// <summary>
        /// Цена продажи
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                _salePrice = value;
                OnPropertyChanged(nameof(SalePrice));
            }
        }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }

        public Revaluation Revaluation { get; set; }

        #endregion
    }
}
