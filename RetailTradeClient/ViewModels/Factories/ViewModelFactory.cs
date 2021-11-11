using RetailTradeClient.State.Navigators;
using RetailTradeClient.ViewModels.Base;
using System;

namespace RetailTradeClient.ViewModels.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        #region Private Members

        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;

        #endregion

        #region Constructor

        public ViewModelFactory(CreateViewModel<LoginViewModel> createLoginViewModel,
            CreateViewModel<HomeViewModel> createHomeViewModel)
        {
            _createLoginViewModel = createLoginViewModel;
            _createHomeViewModel = createHomeViewModel;
        }

        #endregion

        public BaseViewModel CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Home:
                    return _createHomeViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
