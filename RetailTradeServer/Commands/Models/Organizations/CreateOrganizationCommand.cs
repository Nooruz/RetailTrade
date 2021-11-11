using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RetailTradeServer.Commands
{
    public class CreateOrganizationCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly IOrganizationService _organizationService;
        private readonly IUserStore _userStore;
        private OrganizationViewModel _viewModel;

        #endregion

        #region Constructor

        public CreateOrganizationCommand(IOrganizationService organizationService,
            IUserStore userStore,
            OrganizationViewModel viewModel)
        {
            _organizationService = organizationService;
            _userStore = userStore;
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
            _viewModel.ErrorMessage = string.Empty;

            try
            {
                Organization organization = new()
                {
                    Name = _viewModel.Name,
                    Address = _viewModel.Address,
                    FullName = _viewModel.FullName,
                    Phone = _viewModel.Phone,
                    Inn = _viewModel.Inn
                };
                await _organizationService.CreateAsync(organization);
            }
            catch (Exception e)
            {
                _viewModel.ErrorMessage = e.Message;
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
