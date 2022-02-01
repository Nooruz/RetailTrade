using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Unit : DomainObject
    {
        #region Private Members

        private string _longName;
        private string _shortName;

        #endregion

        #region Public Properties

        public string LongName
        {
            get => _longName;
            set
            {
                _longName = value;
                OnPropertyChanged(nameof(LongName));
            }
        }
        public string ShortName
        {
            get => _shortName;
            set
            {
                _shortName = value;
                OnPropertyChanged(nameof(ShortName));
            }
        }

        public ICollection<Product> Products { get; set; }

        #endregion        
    }
}
