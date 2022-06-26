namespace RetailTrade.Domain.Views
{
    public class ProductView : ViewObject
    {
        #region Private Members

        private string _name;
        private string _unit;
        private string _barcode;
        private decimal _purchasePrice;
        private decimal _retailPrice;
        private string _tnved;
        private bool _deleteMark;
        private string _typeProduct;

        #endregion

        #region Public Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged(nameof(Unit));
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
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged(nameof(PurchasePrice));
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
        public string TNVED
        {
            get => _tnved;
            set
            {
                _tnved = value;
                OnPropertyChanged(nameof(TNVED));
            }
        }
        public bool DeleteMark
        {
            get => _deleteMark;
            set
            {
                _deleteMark = value;
                OnPropertyChanged(nameof(DeleteMark));
            }
        }
        public string TypeProduct
        {
            get => _typeProduct;
            set
            {
                _typeProduct = value;
                OnPropertyChanged(nameof(TypeProduct));
            }
        }

        #endregion
    }
}
