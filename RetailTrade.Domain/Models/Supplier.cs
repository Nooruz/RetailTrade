using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class Supplier : DomainObject
    {
        #region Private Members

        private string _fullName;
        private string _shortName;

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
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Inn { get; set; }
        public DateTime CreateDate { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<OrderToSupplier> Orders { get; set; }

        #endregion
    }
}
