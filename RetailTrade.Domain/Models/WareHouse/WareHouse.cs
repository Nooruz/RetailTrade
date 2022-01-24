using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class WareHouse : DomainObject
    {
        #region Private Members

        private string _name;
        private string _address;
        private int _typeWareHouseId;

        #endregion

        #region Public Properties

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        /// <summary>
        /// Тип склада
        /// </summary>
        public int TypeWareHouseId
        {
            get => _typeWareHouseId;
            set
            {
                _typeWareHouseId = value;
                OnPropertyChanged(nameof(TypeWareHouseId));
            }
        }
        public TypeWareHouse TypeWareHouse { get; set; }
        public ICollection<Product> Products { get; set; }

        #endregion        
    }
}
