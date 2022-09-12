namespace RetailTrade.Domain.Models
{
    public class MoveProduct : DomainObject
    {
        #region Private Members

        private int _productId;
        private int _documentId;
        private double _quantity;
        private double _stock;
        private decimal _price;
        private decimal _amount;
        private string _comment;

        #endregion

        #region Public Properties

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

        public int DocumentId
        {
            get => _documentId;
            set
            {
                _documentId = value;
                OnPropertyChanged(nameof(DocumentId));
            }
        }

        /// <summary>
        /// Количество для списания
        /// </summary>
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                Amount = (decimal)Quantity * Price;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public double Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                OnPropertyChanged(nameof(Stock));
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                Amount = Price * (decimal)Quantity;
                OnPropertyChanged(nameof(Price));
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
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

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }

        public Document Document { get; set; }

        #endregion
    }
}
