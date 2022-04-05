namespace RetailTrade.Domain.Models
{
    public class LabelPriceTag : DomainObject
    {
        #region Private Members

        private string _name;
        private int _typeLabelPriceTagId;
        private int _width;
        private int _height;

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

        public int TypeLabelPriceTagId
        {
            get => _typeLabelPriceTagId;
            set
            {
                _typeLabelPriceTagId = value;
                OnPropertyChanged(nameof(TypeLabelPriceTagId));
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public TypeLabelPriceTag TypeLabelPriceTag { get; set; }

        #endregion
    }
}
