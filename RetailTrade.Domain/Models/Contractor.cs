namespace RetailTrade.Domain.Models
{
    public class Contractor : DomainObject
    {
        #region Private Members

        private string _fullName;
        private string _shortName;
        private string _tIN;
        private string _oKPO;
        private string _email;
        private string _phone;
        private int? _consumerId;
        private int _contractorTypeId;

        #endregion

        #region Public Properties

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
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

        public string TIN
        {
            get => _tIN;
            set
            {
                _tIN = value;
                OnPropertyChanged(nameof(TIN));
            }
        }

        public string OKPO
        {
            get => _oKPO;
            set
            {
                _oKPO = value;
                OnPropertyChanged(nameof(OKPO));
            }
        }

        #endregion
    }
}
