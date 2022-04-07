namespace RetailTrade.Domain.Models
{
    public class LabelPriceTag : DomainObject
    {
        #region Private Members

        private string _name;
        private int _typeLabelPriceTagId;
        private int? _labelPriceTagSizeId;

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

        public int? LabelPriceTagSizeId
        {
            get => _labelPriceTagSizeId;
            set
            {
                _labelPriceTagSizeId = value;
                OnPropertyChanged(nameof(LabelPriceTagSizeId));
            }
        }

        public TypeLabelPriceTag TypeLabelPriceTag { get; set; }
        public LabelPriceTagSize LabelPriceTagSize { get; set; }

        #endregion
    }
}
