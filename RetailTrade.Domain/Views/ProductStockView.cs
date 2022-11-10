using System;

namespace RetailTrade.Domain.Views
{
    public class ProductStockView : ViewOnlyNotifyPropertyCahnged
    {
        #region Private Members

        private int _productId;
        private double _quantity;
        private string _wareHouseName;
        private int _wareHouseId;
        private int _documentId;

        #endregion

        #region Puiblic Properties

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

        public string WareHouseName
        {
            get => _wareHouseName;
            set
            {
                _wareHouseName = value;
                OnPropertyChanged(nameof(WareHouseName));
            }
        }

        public int WareHouseId
        {
            get => _wareHouseId;
            set
            {
                _wareHouseId = value;
                OnPropertyChanged(nameof(WareHouseId));
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

        #endregion
    }
}
