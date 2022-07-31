namespace RetailTrade.Domain.Views
{
    public class WareHouseView : ViewObject
    {
        #region Private Members

        private string _name;
        private double _quantity;

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

        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        #endregion
    }
}
