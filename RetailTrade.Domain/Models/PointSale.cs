namespace RetailTrade.Domain.Models
{
    public class PointSale : DomainObject
    {
        #region Private Members

        private string _name;
        private bool _isEnabled = true;

        #endregion

        #region Public Properties

        /// <summary>
        /// Наименование
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

        /// <summary>
        /// Если точка продаж выключена, оформлять новые продажи с нее нельзя.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        #endregion
    }
}
