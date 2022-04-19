using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class Sale : INotifyPropertyChanged
    {
        #region Private Members

        private int _id;
        private string _name;
        private double _quantity;
        private decimal _salePrice;
        private decimal _arrivalPrice;
        private double _quantityInStock;
        private decimal _amountWithoutDiscount;
        private decimal _discountAmount;
        private decimal _total;
        private string _tnved;
        private string _barcode;
        private string _unitName;

        #endregion

        #region Public Properties

        /// <summary>
        /// Код товара
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Количество покупки
        /// </summary>
        public double Quantity
        {
            get => _quantity;
            set
            {                
                if (value > 0)
                {
                    _quantity = value;
                    AmountWithoutDiscount = SalePrice * (decimal)_quantity;
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }

        /// <summary>
        /// Цена продажи
        /// </summary>
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
        /// Цена прихода
        /// </summary>
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
        /// Итого
        /// </summary>
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        /// <summary>
        /// Количество на складе
        /// </summary>
        public double QuantityInStock
        {
            get => _quantityInStock;
            set
            {
                _quantityInStock = value;
                OnPropertyChanged(nameof(QuantityInStock));
            }
        }

        /// <summary>
        /// Сумма без скидки
        /// </summary>
        public decimal AmountWithoutDiscount
        {
            get => _amountWithoutDiscount;
            set
            {
                _amountWithoutDiscount = value;
                Total = _amountWithoutDiscount;
                OnPropertyChanged(nameof(AmountWithoutDiscount));
            }
        }

        /// <summary>
        /// Сумма скидки
        /// </summary>
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                _discountAmount = value;
                Total -= _discountAmount;
                OnPropertyChanged(nameof(DiscountAmount));
            }
        }

        public string TNVED
        {
            get => _tnved;
            set
            {
                _tnved = value;
                OnPropertyChanged(nameof(TNVED));
            }
        }

        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }

        public string UnitName
        {
            get => _unitName;
            set
            {
                _unitName = value;
                OnPropertyChanged(nameof(UnitName));
            }
        }

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
