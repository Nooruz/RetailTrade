using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class WareHouse : DomainObject
    {
        #region Private Members

        private string _name;
        private string _address;
        private int _typeWareHouseId;
        private bool _deleteMark;

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

        /// <summary>
        /// Пометка на удаление по умолчанию false
        /// </summary>
        public bool DeleteMark
        {
            get => _deleteMark;
            set
            {
                _deleteMark = value;
                OnPropertyChanged(nameof(DeleteMark));
            }
        }

        public TypeWareHouse TypeWareHouse { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Arrival> Arrivals { get; set; }
        public ICollection<ProductWareHouse> ProductsWareHouses { get; set; }
        public ICollection<ArrivalProduct> ArrivalProducts { get; set; }
        public ICollection<ProductSale> ProductSales { get; set; }

        #endregion        
    }
}
