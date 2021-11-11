using System;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Приход товара
    /// </summary>
    public class ArrivalProduct : DomainObject, INotifyPropertyChanged
    {
        #region Private Members

        private decimal _quantity;
        private int _productId;
        private Product _product;

        #endregion

        #region Public Properties

        /// <summary>
        /// Дата прихода
        /// </summary>
        public DateTime ArrivalDate { get; set; }

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
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

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

        #endregion

        #region Constructor

        public ArrivalProduct()
        {

        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
