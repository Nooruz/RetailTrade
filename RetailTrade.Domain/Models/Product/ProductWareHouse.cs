using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class ProductWareHouse : INotifyPropertyChanged
    {
        #region Private Members

        private int _productId;
        private int _wareHouseId;

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
        public int WareHouseId
        {
            get => _wareHouseId;
            set
            {
                _wareHouseId = value;
                OnPropertyChanged(nameof(WareHouseId));
            }
        }
        public Product Product { get; set; }
        public WareHouse WareHouse { get; set; }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
