namespace RetailTrade.Domain.Views
{
    public class ProductSaleView : ViewObject
    {
        #region Private Members

        private int _productId;
        private double _quantity;
        private decimal _total;
        private decimal _retailPrice;
        private decimal _purchasePrice;
        private decimal _discountAmount;
        private decimal _totalWithDiscount;
        private string _name;
        private int _receiptId;

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
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }
        public decimal RetailPrice
        {
            get => _retailPrice;
            set
            {
                _retailPrice = value;
                OnPropertyChanged(nameof(RetailPrice));
            }
        }
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged(nameof(PurchasePrice));
            }
        }
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                _discountAmount = value;
                OnPropertyChanged(nameof(DiscountAmount));
            }
        }
        public decimal TotalWithDiscount
        {
            get => _totalWithDiscount;
            set
            {
                _totalWithDiscount = value;
                OnPropertyChanged(nameof(TotalWithDiscount));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int ReceiptId
        {
            get => _receiptId;
            set
            {
                _receiptId = value;
                OnPropertyChanged(nameof(ReceiptId));
            }
        }

        #endregion
    }
}
