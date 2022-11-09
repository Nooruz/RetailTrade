using System;

namespace RetailTrade.Domain.Views
{
    public class ProductOutcomingHistoryView : ViewOnlyNotifyPropertyCahnged
    {
        #region Private Members

        private int _productId;
        private string _documentType;
        private DateTime _createdDate;
        private double _quantity;
        private decimal _price;
        private decimal _amount;
        private string _number;

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
        public string DocumentType
        {
            get => _documentType;
            set
            {
                _documentType = value;
                OnPropertyChanged(nameof(DocumentType));
            }
        }
        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged(nameof(CreatedDate));
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
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        #endregion
    }
}
