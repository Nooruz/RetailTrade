using RetailTradeServer.Commands;
using RetailTradeServer.Properties;
using RetailTradeServer.State.Authenticators;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System.Reflection;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Private Members

        private readonly INavigator _navigator;
        private readonly IAuthenticator _authenticator;
        private bool _isConnected;

        #endregion

        #region Public Properties

        public BaseViewModel CurrentViewModel => _navigator.CurrentViewModel;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }
        public string WindowTitle => $"SP: Магазин, версия {Assembly.GetExecutingAssembly().GetName().Version}";

        #endregion

        #region Command

        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand UsersCommand { get; }
        public ICommand CurrencyCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand CloseModalCommand { get; }

        #endregion

        #region Constructor

        public MainViewModel(INavigator navigator,
            IRetailTradeViewModelFactory retailTradeViewModelFactory,
            IAuthenticator authenticator)
        {
            _navigator = navigator;
            _authenticator = authenticator;

            //AboutCommand = new RelayCommand(About);
            CloseModalCommand = new RelayCommand(CloseModal);

            _navigator.StateChanged += Navigator_StateChanged;
            _authenticator.StateChanged += AuthenticatorStateChanged;

            Settings.Default.AdminCreated = true;
            Settings.Default.Save();

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, retailTradeViewModelFactory);
            UpdateCurrentViewModelCommand.Execute(Settings.Default.AdminCreated ? ViewType.Login : ViewType.Registration);
        }

        #endregion

        #region Private Voids        
        
        private void AuthenticatorStateChanged()
        {
            OnPropertyChanged(nameof(IsLogin));
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void CloseModal()
        {
            IsModalOpen = false;
        }

        #endregion
    }
}
