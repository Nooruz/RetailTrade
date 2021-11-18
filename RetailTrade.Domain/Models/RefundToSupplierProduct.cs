using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class RefundToSupplierProduct : DomainObject, INotifyPropertyChanged
    {
        #region Private Members

        private decimal _quantity;
        private Product _product;

        #endregion

        #region Public Properties

        public int ProductId { get; set; }
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

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
