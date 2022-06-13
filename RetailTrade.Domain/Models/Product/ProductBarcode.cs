namespace RetailTrade.Domain.Models
{
    public class ProductBarcode : DomainObject
    {
        #region Private Members

        private string _barcode;
        private int _productId;

        #endregion

        #region Public Voids

        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }
        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }
        public Product Product { get; set; }

        #endregion
    }
}
