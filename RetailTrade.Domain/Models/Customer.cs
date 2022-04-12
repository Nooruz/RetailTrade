using System;

namespace RetailTrade.Domain.Models
{
    public class Customer : DomainObject
    {
        #region Private Members

        private string _surname;
        private string _name;
        private string _patronymic;
        private string _phone;
        private string _email;
        private DateTime _created;
        private string _tIN;

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

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public DateTime Created
        {
            get => _created;
            set
            {
                _created = value;
                OnPropertyChanged(nameof(Created));
            }
        }

        public string TIN
        {
            get => _tIN;
            set
            {
                _tIN = value;
                OnPropertyChanged(nameof(TIN));
            }
        }

        #endregion
    }
}
