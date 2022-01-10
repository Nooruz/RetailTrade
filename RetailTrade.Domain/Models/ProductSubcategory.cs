using System.Collections.Generic;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class ProductSubcategory : DomainObject
    {
        #region Private Members

        private int _productCategoryId;
        private string _name;
        private ProductCategory _productCategory;

        #endregion

        #region Public Properties

        public int ProductCategoryId
        {
            get => _productCategoryId;
            set
            {
                _productCategoryId = value;
                OnPropertyChanged(nameof(ProductCategoryId));
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
        public ProductCategory ProductCategory
        {
            get => _productCategory;
            set
            {
                _productCategory = value;
                OnPropertyChanged(nameof(ProductCategory));
            }
        }
        public ICollection<Product> Products { get; set; }

        #endregion
    }
}
