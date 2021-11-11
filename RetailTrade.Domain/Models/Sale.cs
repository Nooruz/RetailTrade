using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class Sale : INotifyPropertyChanged
    {
        #region Private Members

        private int _id;
        private string _name;
        private decimal _quantity;
        private decimal _sum;

        #endregion

        #region Public Properties

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
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
        public decimal Quantity
        {
            get => _quantity;
            set
            {                
                if (value > 0)
                {
                    _quantity = value;
                    Sum = SalePrice * _quantity;
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public decimal SalePrice { get; set; }
        public decimal Sum
        {
            get => _sum;
            set
            {                
                _sum = value;
                OnPropertyChanged(nameof(Sum));
            }
        }

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
