namespace RetailTrade.Domain.Models
{
    public class UserPointSale : WithoutKeyDomainObject
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
    }
}
