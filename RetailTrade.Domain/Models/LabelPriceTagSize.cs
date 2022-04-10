using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class LabelPriceTagSize : DomainObject
    {
        #region Private Members

        private int _width;
        private int _height;
        private int _typeLabelPriceTagId;

        #endregion

        #region Public Properties

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
        public int TypeLabelPriceTagId
        {
            get => _typeLabelPriceTagId;
            set
            {
                _typeLabelPriceTagId = value;
                OnPropertyChanged(nameof(TypeLabelPriceTagId));
            }
        }
        public TypeLabelPriceTag TypeLabelPriceTag { get; set; }
        public ICollection<LabelPriceTag> LabelPriceTags { get; set; }

        #endregion
    }
}
