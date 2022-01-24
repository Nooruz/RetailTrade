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
        private bool _isGroup;
        private int? _subGroupId;

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
        public int? SubGroupId
        {
            get => _subGroupId;
            set
            {
                _subGroupId = value;
                OnPropertyChanged(nameof(SubGroupId));
            }
        }
        public TypeProduct SubGroup { get; set; }
        public ICollection<TypeProduct> SubGroups { get; set; }
        public ICollection<Product> Products { get; set; }

        #endregion        
    }
}
