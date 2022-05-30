using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.Report;
using System.Collections.Generic;
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
        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly IUnitService _unitService;
        private readonly IRevaluationProductService _revaluationProductService;
        private ObservableCollection<Revaluation> _revaluations = new();
        private ObservableCollection<RevaluationProduct> _revaluationProducts = new();
        private ObservableCollection<Product> _products = new();
        private Revaluation _selectedRevaluation;

        #endregion

        #region Public Properties

        public ObservableCollection<Revaluation> Revaluations
        {
            get => _revaluations;
            set
            {
                _revaluations = value;
                OnPropertyChanged(nameof(Revaluations));
            }
        }
        public ObservableCollection<RevaluationProduct> RevaluationProducts
        {
            get => _revaluationProducts;
            set
            {
                _revaluationProducts = value;
                OnPropertyChanged(nameof(RevaluationProducts));
            }
        }
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
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
            IUnitService unitService,
            IRevaluationProductService revaluationProductService)
        {
            _revaluationService = revaluationService;
            _menuNavigator = menuNavigator;
            _productService = productService;
            _typeProductService = typeProductService;
            _unitService = unitService;
            _revaluationProductService = revaluationProductService;

            Header = "История изменения цен";

            CreateCommand = new RelayCommand(() => _menuNavigator.AddViewModel(new SettingProductPriceViewModel(productService, typeProductService, revaluationService, unitService, menuNavigator) { Header = "Установка цен товаров (создание) *" }));

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

        public async void Print()
        {
            if (SelectedRevaluation != null)
            {
                ShowLoadingPanel = true;
                RevaluationReport report = new(SelectedRevaluation)
                {
                    DataSource = await _revaluationProductService.GetAllByRevaluationId(SelectedRevaluation.Id)
                };
                await report.CreateDocumentAsync();
                DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel { PrintingDocument = report } );
                ShowLoadingPanel = false;
            }
            else
            {
                _ = MessageBoxService.Show("Выберите историю изменения цен товаров!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
