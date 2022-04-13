using System;

namespace RetailTrade.Domain.Models
{
    public class Customer : Contractor
    {
        #region Private Members

        private string _surname;
        private string _name;
        private string _patronymic;
        private DateTime? _birthdate;
        private int? _genderId;
        private string _address;

        #endregion

        #region Public Properties

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
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

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged(nameof(Patronymic));
            }
        }

        public DateTime? Birthdate
        {
            get => _birthdate;
            set
            {
                _birthdate = value;
                OnPropertyChanged(nameof(Birthdate));
            }
        }

        public int? GenderId
        {
            get => _genderId;
            set
            {
                _genderId = value;
                OnPropertyChanged(nameof(GenderId));
            }
        }

        public Gender Gender { get; set; }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        #endregion
    }
}
