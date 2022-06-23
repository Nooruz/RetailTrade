using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using DevExpress.XtraEditors.DXErrorProvider;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SettingProductPriceViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly IRevaluationService _revaluationService;
        private readonly IDataService<Unit> _unitService;
        private readonly IMenuNavigator _menuNavigator;
        private ObservableCollection<RevaluationProduct> _revaluationProducts = new();
        private IEnumerable<Unit> _units;
        private RevaluationProduct _selectedRevaluationProduct;
        private ObservableCollection<Product> _products;
        private ObservableCollection<Product> _filteringProducts;
        private string _productNameFilterCriteria;
        private string _comment;

        #endregion

        #region Public Properties

        public ObservableCollection<Product> Products
        {
            get => _products ?? new();
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<Product> FilteringProducts
        {
            get => _filteringProducts ?? new();
            set
            {
                _filteringProducts = value;
                OnPropertyChanged(nameof(FilteringProducts));
            }
        }
        public RevaluationProduct SelectedRevaluationProduct
        {
            get => _selectedRevaluationProduct;
            set
            {
                _selectedRevaluationProduct = value;
                OnPropertyChanged(nameof(SelectedRevaluationProduct));
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
        public IEnumerable<Unit> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        private RevaluationProduct EmptyRevaluationProduct => RevaluationProducts.FirstOrDefault(r => r.ProductId == 0);
        public string ProductNameFilterCriteria
        {
            get => _productNameFilterCriteria;
            set
            {
                _productNameFilterCriteria = value;
                OnPropertyChanged(nameof(ProductNameFilterCriteria));
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        public GridControl ProductGridControl { get; set; }
        public TableView ProductTableView => ProductGridControl != null ? ProductGridControl.View as TableView : null;

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand AddProductCommand => new RelayCommand(AddProduct);
        public ICommand ProductCommand => new RelayCommand(Create);
        public ICommand CellValueChangedCommand => new ParameterCommand(CellValueChanged);
        public ICommand ValidateCellCommand => new ParameterCommand(ValidateCell);
        public ICommand CreateAndCloseCommand => new RelayCommand(CreateAndClose);
        public ICommand DeleteRevaluationProductCommand => new RelayCommand(DeleteRevaluationProduct);
        public ICommand ProductGridControlLoadedCommand => new ParameterCommand((object p) => ProductGridControlLoaded(p));

        #endregion

        #region Constructor

        public SettingProductPriceViewModel(IProductService productService,
            ITypeProductService typeProductService,
            IRevaluationService revaluationService,
            IDataService<Unit> unitService,
            IMenuNavigator menuNavigator)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _revaluationService = revaluationService;
            _unitService = unitService;
            _menuNavigator = menuNavigator;
        }

        #endregion

        #region Private Voids

        private void ProductGridControlLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    ProductGridControl = gridControl;
                    ProductTableView.ShownEditor += ProductTableView_ShownEditor;
                }
            }
        }

        private void ProductTableView_ShownEditor(object sender, EditorEventArgs e)
        {
            ProductTableView.Grid.View.ActiveEditor.SelectAll();
        }

        private void ShowEditor(int column)
        {
            ProductTableView.FocusedRowHandle = RevaluationProducts.IndexOf(SelectedRevaluationProduct);
            ProductTableView.Grid.CurrentColumn = ProductTableView.Grid.Columns[column];
            _ = ProductTableView.Dispatcher.BeginInvoke(new Action(() =>
            {
                ProductTableView.ShowEditor();
            }), DispatcherPriority.Render);
        }

        private void DeleteRevaluationProduct()
        {
            if (SelectedRevaluationProduct != null)
            {
                _ = RevaluationProducts.Remove(SelectedRevaluationProduct);
            }
        }

        private async void CreateAndClose()
        {
            if (RevaluationProducts.Any())
            {
                if (EmptyRevaluationProduct == null)
                {
                    RevaluationProduct revaluationProduct = RevaluationProducts.FirstOrDefault(r => r.ArrivalPrice <= 0);
                    if (revaluationProduct == null)
                    {
                        revaluationProduct = RevaluationProducts.FirstOrDefault(r => r.SalePrice <= 0);
                        if (revaluationProduct == null)
                        {
                            RevaluationProducts.ToList().ForEach(r =>
                            {
                                r.OldArrivalPrice = r.Product.PurchasePrice;
                                r.OldSalePrice = r.Product.RetailPrice;
                                r.Product = null;                                
                            });
                            _ = await _revaluationService.CreateAsync(new Revaluation
                            {
                                RevaluationDate = DateTime.Now,
                                Comment = Comment,
                                RevaluationProducts = RevaluationProducts
                            });
                            _menuNavigator.DeleteViewModel(this);
                        }
                        else
                        {
                            _ = MessageBoxService.Show("Введите цену продажи.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            SelectedRevaluationProduct = revaluationProduct;
                            ShowEditor(5);
                        }
                    }
                    else
                    {
                        _ = MessageBoxService.Show("Введите цену прихода.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        SelectedRevaluationProduct = revaluationProduct;
                        ShowEditor(4);
                    }
                }
                else
                {
                    _ = MessageBoxService.Show("Товар не выбран.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    SelectedRevaluationProduct = EmptyRevaluationProduct;
                    ShowEditor(0);
                }
            }
            else
            {
                _ = MessageBoxService.Show("Не введено ни одной строки в список.", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (e.Cell.Property == nameof(RevaluationProduct.ProductId))
                {
                    try
                    {
                        if (RevaluationProducts.Any(r => r.ProductId == (int)e.Value))
                        {
                            if (SelectedRevaluationProduct.ProductId != (int)e.Value)
                            {
                                e.ErrorContent = "Такой товар уже введен.";
                                e.ErrorType = ErrorType.Critical;
                                e.IsValid = false;
                            }
                            else
                            {
                                ShowEditor(4);
                            }
                        }
                        else
                        {
                            ShowEditor(4);
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
                if (e.Cell.Property == nameof(RevaluationProduct.ArrivalPrice))
                {
                    ShowEditor(5);
                }
            }
        }

        private void CellValueChanged(object parameter)
        {
            if (parameter is CellValueChangedEventArgs e)
            {
                if (e.Cell.Property == nameof(RevaluationProduct.ProductId))
                {
                    try
                    {
                        Product selectedProduct = Products.FirstOrDefault(p => p.Id == (int)e.Cell.Value);
                        if (selectedProduct != null)
                        {
                            SelectedRevaluationProduct.Product = selectedProduct;
                            ShowEditor(4);
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
        }

        private void Create()
        {
            ProductDialogFormModel viewModel = new(_typeProductService) { Products = Products };
            viewModel.OnProductSelected += ProductDialogFormModel_OnProductSelected;
            viewModel.OnProductsSelected += ProductDialogFormModel_OnProductsSelected;
            WindowService.Show(nameof(ProductDialogForm), viewModel);            
        }

        private void ProductDialogFormModel_OnProductSelected(Product product)
        {
            if (product != null)
            {
                if (RevaluationProducts.FirstOrDefault(r => r.ProductId == product.Id) == null)
                {
                    EmptyRevaluationProduct.Product = product;
                    EmptyRevaluationProduct.ProductId = product.Id;
                    ShowEditor(4);
                }
                else
                {
                    _ = MessageBoxService.Show("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);                    
                }
            }
        }

        private void ProductDialogFormModel_OnProductsSelected(IEnumerable<Product> products)
        {
            products.ToList().ForEach(product =>
            {
                try
                {
                    if (RevaluationProducts.FirstOrDefault(r => r.ProductId == product.Id) == null)
                    {
                        EmptyRevaluationProduct.Product = product;
                        EmptyRevaluationProduct.ProductId = product.Id;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            });
        }

        private void AddProduct()
        {
            if (EmptyRevaluationProduct != null)
            {
                _ = MessageBoxService.Show("Товар не выбран.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                SelectedRevaluationProduct = EmptyRevaluationProduct;
                ShowEditor(0);
                return;
            }
            if (RevaluationProducts.Any())
            {
                RevaluationProduct revaluationProduct = RevaluationProducts.FirstOrDefault(r => r.ArrivalPrice <= 0);
                if (revaluationProduct != null)
                {
                    _ = MessageBoxService.Show("Введит цену прихода.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    SelectedRevaluationProduct = revaluationProduct;
                    ShowEditor(4);
                    return;
                }
                revaluationProduct = RevaluationProducts.FirstOrDefault(r => r.SalePrice <= 0);
                if (revaluationProduct != null)
                {
                    _ = MessageBoxService.Show("Введит цену продажи.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    SelectedRevaluationProduct = revaluationProduct;
                    ShowEditor(5);
                    return;
                }
            }
            RevaluationProducts.Add(new RevaluationProduct());
            SelectedRevaluationProduct = EmptyRevaluationProduct;
            ShowEditor(0);
        }

        private async void UserControlLoaded()
        {
            Units = await _unitService.GetAllAsync();
            Products = new(await _productService.GetAllUnmarkedAsync());
            ShowLoadingPanel = false;
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
