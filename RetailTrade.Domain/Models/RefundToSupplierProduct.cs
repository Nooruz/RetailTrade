namespace RetailTrade.Domain.Models
{
    public class RefundToSupplierProduct : DomainObject
    {
        #region Private Members

        private double _quantity;
        private Product _product;
        private int _productId;

        #endregion

        #region Public Properties

        public int RefundToSupplierId { get; set; }
        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductId));
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
    }
}
