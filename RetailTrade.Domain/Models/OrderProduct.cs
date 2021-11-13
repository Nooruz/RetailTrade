using System.Collections.Generic;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class OrderProduct : DomainObject, INotifyPropertyChanged
    {
        #region Private Members

        private int _productId;
        private decimal _quantity;
        private Product _product;

        #endregion

        #region Public Properties

        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }
        public int OrderToSupplierId { get; set; }
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }
        public OrderToSupplier OrderToSupplier { get; set; }

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
