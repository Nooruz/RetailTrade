using System.Collections.Generic;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class ProductCategory : DomainObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Категории товаров
        /// </summary>
        public string Name { get; set; }

        public List<ProductSubcategory> ProductSubcategories { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
