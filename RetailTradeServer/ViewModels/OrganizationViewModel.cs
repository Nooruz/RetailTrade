using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class OrganizationViewModel : BaseViewModel
    {
        #region Private Members

        private string _name;
        private string _address;
        private string _fullName;
        private string _phone;
        private string _inn;

        #endregion

        #region Public Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public string Inn
        {
            get => _inn;
            set
            {
                _inn = value;
                OnPropertyChanged(nameof(Inn));
                OnPropertyChanged(nameof(CanCreate));
            }
        }
        public bool CanCreate => !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Address) &&
            !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Inn) &&
            !string.IsNullOrEmpty(Phone);

        #endregion

        #region Commands

        public ICommand CreateOrganizationCommand { get; }

        #endregion

        #region Constructor

        public OrganizationViewModel(IOrganizationService organizationService,
            IRetailTradeViewModelFactory viewModelFactory,
            INavigator navigator)
        {
            CreateOrganizationCommand = new CreateOrganizationCommand(organizationService, viewModelFactory, navigator, this);
        }

        #endregion
    }
}
