using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Вид товара
    /// </summary>
    public class TypeProduct : DomainObject
    {
        #region Private Members

        private string _name;
        private bool _isGroup = true;
        private int _productId;

        #endregion

        #region Public Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public bool IsGroup
        {
            get => _isGroup;
            set
            {
                _isGroup = value;
                OnPropertyChanged(nameof(IsGroup));
            }
        }
        public TypeProduct SubGroup { get; set; }
        public int? SubGroupId { get; set; }
        public ICollection<TypeProduct> SubGroups { get; set; }
        public ICollection<Product> Products { get; set; }

        #endregion        
    }
}
