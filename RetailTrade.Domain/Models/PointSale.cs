using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class PointSale : DomainObject
    {
        #region Private Members

        private string _name;
        private bool _isEnabled = true;
        private bool _allowDiscount = true;
        private double _maximumDiscount;
        private int _wareHouseId;
        private bool _accountingBalances;
        private string _description;
        private string _address;
        private List<UserPointSale> _userPointSales;

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
        /// Если точка продаж выключена, оформлять новые продажи с нее нельзя.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        /// <summary>
        /// Разрешить скидки, по умолчание равно истину
        /// </summary>
        public bool AllowDiscount
        {
            get => _allowDiscount;
            set
            {
                _allowDiscount = value;
                OnPropertyChanged(nameof(AllowDiscount));
            }
        }

        /// <summary>
        /// Максимальная скидка
        /// </summary>
        public double MaximumDiscount
        {
            get => _maximumDiscount;
            set
            {
                _maximumDiscount = value;
                OnPropertyChanged(nameof(MaximumDiscount));
            }
        }

        /// <summary>
        /// Код склада
        /// </summary>
        public int WareHouseId
        {
            get => _wareHouseId;
            set
            {
                _wareHouseId = value;
                OnPropertyChanged(nameof(WareHouseId));
            }
        }

        /// <summary>
        /// Учет остатков запрещает продавать товары, которых нет на складе.
        /// </summary>
        public bool AccountingBalances
        {
            get => _accountingBalances;
            set
            {
                _accountingBalances = value;
                OnPropertyChanged(nameof(AccountingBalances));
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
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
        /// Склад
        /// </summary>
        public WareHouse WareHouse { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<ProductSale> ProductSales { get; set; }

        public List<UserPointSale> UserPointSale
        {
            get => _userPointSales;
            set
            {
                _userPointSales = value;
                OnPropertyChanged(nameof(UserPointSale));
            }
        }

        #endregion
    }
}
