using System;

namespace RetailTrade.Domain.Models
{
    public class Contractor : DomainObject
    {
        #region Private Members

        private string _fullName;
        private string _workName;
        private string _tIN;
        private string _oKPO;
        private string _email;
        private string _phone;
        private int _contractorTypeId;
        private DateTime _created;
        private string _comment;
        private bool _isDelete;

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

        public string WorkName
        {
            get => _workName;
            set
            {
                _workName = value;
                OnPropertyChanged(nameof(WorkName));
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

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public int ContractorTypeId
        {
            get => _contractorTypeId;
            set
            {
                _contractorTypeId = value;
                OnPropertyChanged(nameof(ContractorTypeId));
            }
        }

        public DateTime Created
        {
            get => _created;
            set
            {
                _created = value;
                OnPropertyChanged(nameof(Created));
            }
        }

        public ContractorType ContractorType { get; set; }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public bool IsDeleted
        {
            get => _isDelete;
            set
            {
                _isDelete = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

        #endregion
    }
}
