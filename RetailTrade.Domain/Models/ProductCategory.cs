using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class ProductCategory : DomainObject, INotifyPropertyChanged
    {
        private ObservableCollection<ProductSubcategory> _productSubcategories;
        /// <summary>
        /// Категории товаров
        /// </summary>
        public string Name { get; set; }

        public ObservableCollection<ProductSubcategory> ProductSubcategories
        {
            get => _productSubcategories;
            set
            {
                _productSubcategories = value;
                OnPropertyChanged(nameof(ProductSubcategories));
            }
        }

        public ProductCategory()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
