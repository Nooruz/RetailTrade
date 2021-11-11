using RetailTrade.Domain.Services;
using RetailTradeClient.Commands;
using RetailTradeClient.State.Navigators;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Base;
using RetailTradeClient.ViewModels.Factories;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Private Members

        private readonly INavigator _navigator;

        #endregion

        #region Public Properties

        public BaseViewModel CurrentViewModel => _navigator.CurrentViewModel;                

        #endregion

        #region Command

        public ICommand UpdateCurrentViewModelCommand { get; }    

        #endregion

        #region Constructor

        public MainViewModel(INavigator navigator,
            IViewModelFactory viewModelFactory,
            IUserStore userStore,
            IOrganizationService organizationService)
        {
            _navigator = navigator;

            _navigator.StateChanged += Navigator_StateChanged;

            userStore.Organization = organizationService.Get();

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, viewModelFactory);

            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        #endregion

        #region Private Voids

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _navigator.StateChanged -= Navigator_StateChanged;

            base.Dispose();
        }

        #endregion
    }
}
