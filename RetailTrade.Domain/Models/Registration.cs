using System;
using System.Collections.ObjectModel;

namespace RetailTrade.Domain.Models
{
    public class Registration : DomainObject
    {
        #region Private Members

        private DateTime _registrationDate;
        private int _wareHouseId;
        private decimal _sum;
        private string _comment;
        private ObservableCollection<EnterProduct> _registrationProducts = new();

        #endregion

        #region Private Members

        public DateTime RegistrationDate
        {
            get => _registrationDate;
            set
            {
                _registrationDate = value;
                OnPropertyChanged(nameof(RegistrationDate));
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
        public decimal Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged(nameof(Sum));
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
        public WareHouse WareHouse { get; set; }
        public ObservableCollection<EnterProduct> RegistrationProducts
        {
            get => _registrationProducts;
            set
            {
                _registrationProducts = value;
                OnPropertyChanged(nameof(RegistrationProducts));
            }
        }

        #endregion
    }
}
