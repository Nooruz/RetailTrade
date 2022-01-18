using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Factories;
using RetailTradeServer.ViewModels.Menus;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailTradeServer.Commands
{
    public class CreateOrganizationCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IOrganizationService _organizationService;
        private OrganizationViewModel _viewModel;

        #endregion

        #region Commands

        public ICommand UpdateCurrentViewModelCommand { get; }

        #endregion

        #region Constructor

        public CreateOrganizationCommand(IOrganizationService organizationService,
            IRetailTradeViewModelFactory viewModelFactory,
            INavigator navigator,
            OrganizationViewModel viewModel)
        {
            _organizationService = organizationService;
            _viewModel = viewModel;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, viewModelFactory);

            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return _viewModel.CanCreate;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                Organization organization = new()
                {
                    Address = _viewModel.Address,
                    FullName = _viewModel.FullName,
                    Phone = _viewModel.Phone,
                    Inn = _viewModel.Inn
                };
                _ = await _organizationService.CreateAsync(organization);
                UpdateCurrentViewModelCommand.Execute(ViewType.Home);
            }
            catch (Exception e)
            {
                //ignore
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.CanCreate))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
