using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Supplier : DomainObject
    {
        #region Private Members

        private string _fullName;
        private string _shortName;
        private string _address;
        private string _phone;
        private string _inn;
        private DateTime _createDate;

        #endregion

        #region Public Properties

        /// <summary>
        /// Полное наименование поставщика
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Краткое наименование поставщика
        /// </summary>
        public string ShortName
        {
            get => _shortName;
            set
            {
                _shortName = value;
                OnPropertyChanged(nameof(ShortName));
            }
        }
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
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
        public string Inn
        {
            get => _inn;
            set
            {
                _inn = value;
                OnPropertyChanged(nameof(Inn));
            }
        }
        public DateTime CreateDate
        {
            get => _createDate;
            set
            {
                _createDate = value;
                OnPropertyChanged(nameof(CreateDate));
            }
        }

        public ICollection<Product> Products { get; set; }
        public ICollection<OrderToSupplier> Orders { get; set; }

        #endregion
    }
}
