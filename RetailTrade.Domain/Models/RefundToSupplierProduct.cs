using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class RefundToSupplierProduct : DomainObject, INotifyPropertyChanged
    {
        #region Private Members

        private double _quantity;
        private Product _product;

        #endregion

        #region Public Properties

        public int RefundToSupplierId { get; set; }
        public int ProductId { get; set; }
        public double Quantity
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
        public RefundToSupplier RefundToSupplier { get; set; }

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
