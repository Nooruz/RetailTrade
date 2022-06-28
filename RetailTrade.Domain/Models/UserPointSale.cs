using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class UserPointSale : INotifyPropertyChanged
    {
        #region Private Members

        private int _userId;
        private int _pointSaleId;

        #endregion

        #region Public Properties

        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        public int PointSaleId
        {
            get => _pointSaleId;
            set
            {
                _pointSaleId = value;
                OnPropertyChanged(nameof(PointSaleId));
            }
        }

        public User User { get; set; }

        public PointSale PointSale { get; set; }

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
