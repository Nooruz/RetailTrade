using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Приход товара
    /// </summary>
    public class ArrivalProduct : DomainObject
    {
        #region Private Members

        private double _quantity;
        private decimal _arrivalPrice;
        private decimal _arrivalSum;
        private int _productId;
        private int _arrivalId;
        private int? _wareHouseId;
        private Product _product;

        #endregion

        #region Public Properties

        public int ArrivalId
        {
            get => _arrivalId;
            set
            {
                _arrivalId = value;
                OnPropertyChanged(nameof(ArrivalId));
            }
        }

        public int? WareHouseId
        {
            get => _wareHouseId;
            set
            {
                _wareHouseId = value;
                OnPropertyChanged(nameof(WareHouseId));
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
        /// Количество прихода
        /// </summary>
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                ArrivalSum = ArrivalPrice * (decimal)Quantity;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(ArrivalSum));
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
                ArrivalSum = ArrivalPrice * (decimal)Quantity;
                OnPropertyChanged(nameof(ArrivalPrice));
                OnPropertyChanged(nameof(ArrivalSum));
            }
        }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrivalSum
        {
            get => _arrivalSum;
            set
            {
                _arrivalSum = value;
                OnPropertyChanged(nameof(ArrivalSum));
            }
        }

        /// <summary>
        /// Цена продажи
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public Arrival Arrival { get; set; }

        public WareHouse WareHouse { get; set; }

        #endregion
    }
}
