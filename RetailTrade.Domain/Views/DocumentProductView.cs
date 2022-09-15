using System;

namespace RetailTrade.Domain.Views
{
    public class DocumentProductView : ViewObject
    {
        #region Private Members

        private int _id;
        private string _number;
        private DateTime _createdDate;
        private string _userName;
        private decimal _amount;
        private string _wareHouse;
        private string _comment;

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
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
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
        public string Username
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(Username));
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
        public string WareHouse
        {
            get => _wareHouse;
            set
            {
                _wareHouse = value;
                OnPropertyChanged(nameof(WareHouse));
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

        #endregion
    }
}
