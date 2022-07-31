namespace RetailTrade.Domain.Models
{
    public class RegistrationProduct : DomainObject
    {
        #region Private Members

        private int _productId;
        private int _registrationId;
        private double _quantity;
        private decimal _price;
        private string _comment;

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
        public int RegistrationId
        {
            get => _registrationId;
            set
            {
                _registrationId = value;
                OnPropertyChanged(nameof(RegistrationId));
            }
        }
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        public Product Product { get; set; }
        public Registration Registration { get; set; }

        #endregion
    }
}
