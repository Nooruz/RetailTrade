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
        private readonly IRenavigator _homeRenavigator;
        private OrganizationViewModel _viewModel;

        #endregion

        #region Constructor

        public CreateOrganizationCommand(IOrganizationService organizationService,
            IRenavigator homeRenavigator,
            OrganizationViewModel viewModel)
        {
            _organizationService = organizationService;
            _homeRenavigator = homeRenavigator;
            _viewModel = viewModel;

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
                _homeRenavigator.Renavigate();
            }
            catch (Exception e)
            {
                //ignore
            }
            finally
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
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
