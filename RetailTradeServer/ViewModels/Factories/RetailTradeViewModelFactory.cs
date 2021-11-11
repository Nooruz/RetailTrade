using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using System;

namespace RetailTradeServer.ViewModels.Factories
{
    public class RetailTradeViewModelFactory : IRetailTradeViewModelFactory
    {
        #region Private Members

        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<RegistrationViewModel> _createRegistrationViewModel;
        private readonly CreateViewModel<OrganizationViewModel> _createOrganizationViewModel;
        private readonly CreateViewModel<SaleViewModel> _createSaleViewModel;

        #endregion

        #region Constructor

        public RetailTradeViewModelFactory(CreateViewModel<LoginViewModel> createLoginViewModel,
            CreateViewModel<HomeViewModel> createHomeViewModel,
            CreateViewModel<RegistrationViewModel> createRegistrationViewModel,
            CreateViewModel<OrganizationViewModel> createOrganizationViewModel,
            CreateViewModel<SaleViewModel> createSaleViewModel)
        {
            _createLoginViewModel = createLoginViewModel;
            _createHomeViewModel = createHomeViewModel;
            _createRegistrationViewModel = createRegistrationViewModel;
            _createOrganizationViewModel = createOrganizationViewModel;
            _createSaleViewModel = createSaleViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Null:
                    return null;
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Registration:
                    return _createRegistrationViewModel();
                case ViewType.Home:
                    return _createHomeViewModel();
                case ViewType.Organization:
                    return _createOrganizationViewModel();
                case ViewType.Sale:
                    return _createSaleViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
