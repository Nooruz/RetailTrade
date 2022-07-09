using RetailTrade.POS.Commands;
using RetailTrade.POS.States.Authenticators;
using RetailTrade.POS.States.Navigators;
using RetailTrade.POS.ViewModels.Factories;
using System.Windows.Input;

namespace RetailTrade.POS.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Private Members

        private readonly INavigator _navigator;
        private readonly IAuthenticator _authenticator;
        private readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Public Properties

        public BaseViewModel CurrentViewModel => _navigator.CurrentViewModel;

        #endregion

        #region Commands

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(_navigator, _viewModelFactory);

        #endregion

        #region Constructor

        public MainViewModel(INavigator navigator,
            IViewModelFactory viewModelFactory,
            IAuthenticator authenticator)
        {
            _navigator = navigator;
            _authenticator = authenticator;
            _viewModelFactory = viewModelFactory;

            _navigator.StateChanged += () => OnPropertyChanged(nameof(CurrentViewModel));
            _authenticator.StateChanged += Authenticator_StateChanged;

            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        #endregion

        #region Private Voids

        private void Authenticator_StateChanged()
        {

        }

        #endregion

        #region Public Voids



        #endregion
    }
}
