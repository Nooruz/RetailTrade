using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class RevaluationViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IRevaluationService _revaluationService;
        private readonly IMenuNavigator _menuNavigator; 
        private ObservableCollection<Revaluation> _revaluations;
        private Revaluation _selectedRevaluation;

        #endregion

        #region Public Properties

        public ObservableCollection<Revaluation> Revaluations
        {
            get => _revaluations ?? new();
            set
            {
                _revaluations = value;
                OnPropertyChanged(nameof(Revaluations));
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

        public RevaluationViewModel(IProductService productService,
            ITypeProductService typeProductService,
            IRevaluationService revaluationService,
            IMenuNavigator menuNavigator,
            IDataService<Unit> unitService)
        {
            _revaluationService = revaluationService;
            _menuNavigator = menuNavigator;

            Header = "История изменения цен";

            CreateCommand = new RelayCommand(() => _menuNavigator.AddViewModel(new SettingProductPriceViewModel(productService, typeProductService, dialogService, revaluationService, unitService, menuNavigator) { Header = "Установка цен товаров (создание) *" }));

            _revaluationService.OnRevaluationCreated += RevaluationService_OnRevaluationCreated;
        }

        #endregion

        #region Private Voids

        private void RevaluationService_OnRevaluationCreated(Revaluation obj)
        {
            Revaluations.Add(obj);
        }

        private async void UserControlLoaded()
        {
            Revaluations = new(await _revaluationService.GetAllAsync());
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
                _ = MessageBox.Show("Выберите историю изменения цен товаров!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _revaluationService.OnRevaluationCreated -= RevaluationService_OnRevaluationCreated;
            base.Dispose();
        }

        #endregion
    }
}
