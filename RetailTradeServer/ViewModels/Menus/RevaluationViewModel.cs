using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class RevaluationViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IRevaluationService _revaluationService;
        private readonly IDialogService _dialogService;
        private readonly IMenuNavigator _menuNavigator;
        private IEnumerable<Revaluation> _revaluations;
        private Revaluation _selectedRevaluation;

        #endregion

        #region Public Properties

        public IEnumerable<Revaluation> Revaluations
        {
            get => _revaluations;
            set
            {
                _revaluations = value;
                OnPropertyChanged(nameof(Revaluation));
            }
        }
        public Revaluation SelectedRevaluation
        {
            get => _selectedRevaluation;
            set
            {
                _selectedRevaluation = value;
                OnPropertyChanged(nameof(SelectedRevaluation));
            }
        }

        #endregion

        #region Commands

        public ICommand PrintCommand => new RelayCommand(Print);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public RevaluationViewModel(IRevaluationService revaluationService,
            IDialogService dialogService,
            IMenuNavigator menuNavigator,
            IDataService<Unit> unitService)
        {
            _revaluationService = revaluationService;
            _dialogService = dialogService;
            _menuNavigator = menuNavigator;

            Header = "История изменения цен";

            CreateCommand = new RelayCommand(() => _menuNavigator.AddViewModel(new SettingProductPriceViewModel(dialogService, unitService) { Header = "Установка цен товаров (создание) *" }));
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            ShowLoadingPanel = false;
        }

        private void Create()
        {

        }

        public void Print()
        {
            if (SelectedRevaluation != null)
            {

            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите историю изменения цен товаров!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {

            base.Dispose();
        }

        #endregion
    }
}
