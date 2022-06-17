using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class ProductPrice : DomainObject
    {
        #region Private Members

        private decimal _retailPrice;
        private decimal _costPrice;
        private decimal _wholesalePrice;
        private decimal _minimumPrice;

        #endregion

        #region Public Properties

        /// <summary>
        /// Розничная цена
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
        /// Себестоимость цена
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice
        {
            get => _costPrice;
            set
            {
                _costPrice = value;
                OnPropertyChanged(nameof(CostPrice));
            }
        }

        /// <summary>
        /// Оптовая цена
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal WholesalePrice
        {
            get => _wholesalePrice;
            set
            {
                _wholesalePrice = value;
                OnPropertyChanged(nameof(WholesalePrice));
            }
        }

        /// <summary>
        /// Минимальная цена
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumPrice
        {
            get => _minimumPrice;
            set
            {
                _minimumPrice = value;
                OnPropertyChanged(nameof(MinimumPrice));
            }
        }

        public Product Product { get; set; }

        #endregion
    }
}
