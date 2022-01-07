using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class ProductCategory : DomainObject, INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
