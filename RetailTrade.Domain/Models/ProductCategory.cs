using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class ProductCategory : DomainObject
    {
        #region Private Members

        private string _name;

        #endregion

        #region Public Properties

        /// <summary>
        /// Категории товаров
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
        //public ICollection<Product> Products { get; set; }
        public ObservableCollection<ProductSubcategory> ProductSubcategories { get; set; }

        #endregion
    }
}
