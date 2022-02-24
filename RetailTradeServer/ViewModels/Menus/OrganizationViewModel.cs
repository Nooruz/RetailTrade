using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class OrganizationViewModel : BaseViewModel
    {
        #region Private Members

        private string _name;
        private string _address;
        private string _fullName;
        private string _shortName;
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
        public string ShortName
        {
            get => _shortName;
            set
            {
                _shortName = value;
                OnPropertyChanged(nameof(ShortName));
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
            !string.IsNullOrEmpty(ShortName) &&
            !string.IsNullOrEmpty(Inn) &&
            !string.IsNullOrEmpty(Phone);

        #endregion

        #region Commands

        public ICommand CreateOrganizationCommand => new CreateOrganizationCommand(organizationService, homeViewModel, this);

        #endregion

        #region Constructor

        public OrganizationViewModel(IOrganizationService organizationService,
            IRenavigator homeViewModel,
            INavigator navigator)
        {
            Header = "Сведения об организации";
        }

        #endregion
    }
}
