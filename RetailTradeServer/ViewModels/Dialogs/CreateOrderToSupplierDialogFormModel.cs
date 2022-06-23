using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.XtraEditors.DXErrorProvider;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateOrderToSupplierDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly ISupplierService _supplierService;
        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IDataService<Unit> _unitService;
        private readonly IUserStore _userStore;
        private Supplier _selectedSupplier;
        private OrderProduct _selectedOrderProduct;
        private int _selectedOrderStatusId = 1;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private IEnumerable<Unit> _units;
        private ObservableCollection<OrderProduct> _orderProducts;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
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
        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
                OnPropertyChanged(nameof(Products));
                Cleare();
            }
        }
        public int SelectedOrderStatusId
        {
            get => _selectedOrderStatusId;
            set
            {
                _selectedOrderStatusId = value;
                OnPropertyChanged(nameof(SelectedOrderStatusId));
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
        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<OrderProduct> OrderProducts
        {
            get => _orderProducts ?? new();
            set
            {
                _orderProducts = value;
                OnPropertyChanged(nameof(OrderProducts));
            }
        }
        public OrderProduct SelectedOrderProduct
        {
            get => _selectedOrderProduct;
            set
            {
                _selectedOrderProduct = value;
                OnPropertyChanged(nameof(SelectedOrderProduct));                
            }
        }
        public GridControl OrderGridControl { get; set; }
        public TableView OrderTableView => OrderGridControl != null ? OrderGridControl.View as TableView : null;
        public GridColumn OrderGridColumn { get; set; }
        public OrderProduct EditOrderProduct => OrderProducts.FirstOrDefault(p => p.ProductId == 0);

        #endregion

        #region Commands

        public ICommand ValidateCellCommand => new ParameterCommand(parameter => ValidateCell(parameter));
        public ICommand OrderProductCommand => new RelayCommand(CreateOrder);
        public ICommand ClearCommand => new RelayCommand(Cleare);
        public ICommand AddProductToOrderCommand => new RelayCommand(AddProductToOrder);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand CellValueChangedCommand => new ParameterCommand(p => CellValueChanged(p));
        public ICommand DeleteSelectedOrderProductCommand => new RelayCommand(DeleteSelectedOrderProduct);
        public ICommand ProductCommand => new RelayCommand(OpenProductDialog);
        public ICommand GridControlLoadedCommand => new ParameterCommand((object p) => GridControlLoaded(p));
        public ICommand EditValueChangingCommand => new ParameterCommand((object p) => EditValueChanging(p));
        public ICommand BarcodeSearchCommand => new RelayCommand(BarcodeSearch);

        #endregion

        #region Constructor

        public CreateOrderToSupplierDialogFormModel(IProductService productService,
            ITypeProductService typeProductService,
            ISupplierService supplierService,
            IOrderToSupplierService orderToSupplierService,
            IDataService<Unit> unitService,
            IUserStore userStore)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _supplierService = supplierService;
            _orderToSupplierService = orderToSupplierService;
            _unitService = unitService;
            _userStore = userStore;

            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());

            _orderProducts = new();
        }

        #endregion

        #region Private Voids

        /// <summary>
        /// Добавление по штрих коду
        /// </summary>
        private void BarcodeSearch()
        {
            if (SelectedSupplier != null)
            {
                BarcodeSearchDialogFormModel viewModel = new();
                UICommand result = DialogService.ShowDialog(dialogCommands: viewModel.Commands, "Введите штрихкод", nameof(BarcodeSearchDialogForm), viewModel);
                if (result.Id is MessageBoxResult messageResult && messageResult == MessageBoxResult.OK)
                {
                    Product product = Products.FirstOrDefault(p => p.Barcode == viewModel.Barcode);
                    if (product != null)
                    {
                        OrderProducts.Add(new OrderProduct
                        {
                            ProductId = product.Id,
                            Product = product,
                            Quantity = 1
                        });
                        SelectedOrderProduct = OrderProducts.FirstOrDefault(o => o.ProductId == product.Id);
                        ShowEditor(2);
                    }
                }
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Выберите поставщика!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        private void EditValueChanging(object parameter)
        {
            if (OrderProducts.Any(o => o.ProductId != 0))
            {
                if (parameter is EditValueChangingEventArgs e)
                {
                    if (MessageBoxService.ShowMessage("Данные не сохранены. Продолжить?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        Cleare();
                    }
                    else
                    {
                        e.IsCancel = true;
                        e.Handled = true;
                    }
                }                
            }
        }

        private void GridControlLoaded(object parameter)
        {
            if (parameter is RoutedEventArgs e)
            {
                if (e.Source is GridControl gridControl)
                {
                    OrderGridControl = gridControl;
                    OrderTableView.ShownEditor += OrderTableView_ShownEditor;
                }
            }
        }

        private void OrderTableView_ShownEditor(object sender, EditorEventArgs e)
        {
            OrderTableView.Grid.View.ActiveEditor.SelectAll();
        }

        private void ProductDialogFormModel_OnProductSelected(Product product)
        {
            if (product != null)
            {
                OrderProduct selectedOrderProduct = OrderProducts.FirstOrDefault(r => r.ProductId == product.Id);
                if (selectedOrderProduct == null)
                {
                    SelectedOrderProduct.ProductId = product.Id;
                    SelectedOrderProduct.Product = product;
                    SelectedOrderProduct.Quantity = 1;
                    ShowEditor(2);
                }
                else
                {
                    _ = MessageBoxService.Show("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ProductDialogFormModel_OnProductsSelected(IEnumerable<Product> products)
        {
            products.ToList().ForEach(product =>
            {
                try
                {
                    if (OrderProducts.FirstOrDefault(r => r.ProductId == product.Id) == null)
                    {
                        SelectedOrderProduct.ProductId = product.Id;
                        SelectedOrderProduct.Product = product;
                        SelectedOrderProduct.Quantity = 1;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            });
        }

        private void OpenProductDialog()
        {
            ProductDialogFormModel viewModel = new(_typeProductService) { Products = new(Products) };
            viewModel.OnProductSelected += ProductDialogFormModel_OnProductSelected;
            viewModel.OnProductsSelected += ProductDialogFormModel_OnProductsSelected;
            WindowService.Show(nameof(ProductDialogForm), viewModel);
        }

        private void DeleteSelectedOrderProduct()
        {
            if (SelectedOrderProduct != null)
            {
                OrderProducts.Remove(SelectedOrderProduct);
            }
        }

        private void CellValueChanged(object parameter)
        {
            if (parameter is CellValueChangedEventArgs e)
            {
                if (e.Cell.Property == "ProductId")
                {
                    Product product = Products.FirstOrDefault(p => p.Id == (int)e.Value);
                    SelectedOrderProduct.Product = product;
                    SelectedOrderProduct.Quantity = 1;
                    ShowEditor(2);
                }
                OrderTableView.PostEditor();
                OrderTableView.Grid.UpdateTotalSummary();
            }
        }

        private async void UserControlLoaded()
        {
            Suppliers = await _supplierService.GetAllAsync();
            Units = await _unitService.GetAllAsync();
        }

        private void ZebraBarcodeScanner_OnBarcodeEvent(string obj)
        {
            if (!string.IsNullOrEmpty(obj) && Products != null && Products.Any())
            {
                Product product = Products.FirstOrDefault(p => p.Barcode == obj);
                if (product != null && OrderProducts.FirstOrDefault(o => o.ProductId == product.Id) == null)
                {
                    OrderProducts.Add(new OrderProduct()
                    {
                        ProductId = product.Id,
                        Product = product
                    });
                }
            }
        }

        private void AddProductToOrder()
        {
            if (SelectedSupplier != null)
            {
                if (EditOrderProduct != null)
                {
                    _ = MessageBoxService.Show("Выберите товар!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    
                }
                else
                {
                    OrderProducts.Add(new OrderProduct());
                    SelectedOrderProduct = EditOrderProduct;   
                    ShowEditor(0);
                }
            }
            else
            {
                _ = MessageBoxService.Show("Выберите поставщика!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void ShowEditor(int column)
        {
            OrderTableView.FocusedRowHandle = OrderProducts.IndexOf(SelectedOrderProduct);
            OrderTableView.Grid.CurrentColumn = OrderTableView.Grid.Columns[column];
            _ = OrderTableView.Dispatcher.BeginInvoke(new Action(() =>
            {
                OrderTableView.ShowEditor();
            }), DispatcherPriority.Render);
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if (e.Cell.Property == nameof(OrderProduct.Quantity))
                {
                    if ((decimal)e.Value < 0)
                    {
                        _ = MessageBoxService.Show("Количество заказа не должно быть 0.", "", MessageBoxButton.OK, MessageBoxImage.Error);                        
                        e.ErrorContent = "Количество заказа не должно быть 0.";
                        e.ErrorType = ErrorType.Critical;
                        e.IsValid = false;
                    }
                }
                if (e.Cell.Property == nameof(OrderProduct.ProductId))
                {
                    try
                    {
                        if (OrderProducts.Any(r => r.ProductId == (int)e.Value))
                        {
                            if (SelectedOrderProduct.ProductId != (int)e.Value)
                            {
                                _ = MessageBoxService.Show("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                e.ErrorContent = "Такой товар уже введен.";
                                e.ErrorType = ErrorType.Critical;
                                e.IsValid = false;                                
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
        }

        private async void CreateOrder()
        {
            if (OrderProducts.Any())
            {
                OrderProduct orderProduct = OrderProducts.FirstOrDefault(o => o.ProductId == 0);
                if (orderProduct == null)
                {
                    orderProduct = OrderProducts.FirstOrDefault(o => o.Quantity == 0);
                    if (orderProduct == null)
                    {
                        _ = await _orderToSupplierService.CreateAsync(new OrderToSupplier
                        {
                            OrderDate = DateTime.Now,
                            SupplierId = SelectedSupplier.Id,
                            OrderStatusId = 1,
                            Comment = Comment,
                            OrderProducts = OrderProducts.Select(o => new OrderProduct { ProductId = o.ProductId, Quantity = o.Quantity }).ToList()
                        });
                        CurrentWindowService.Close();
                    }
                    else
                    {
                        _ = MessageBoxService.ShowMessage("Введите количество.", "Sale Page", MessageButton.OK, MessageIcon.Error);
                        SelectedOrderProduct = orderProduct;
                        ShowEditor(2);
                    }
                }
                else
                {
                    _ = MessageBoxService.ShowMessage("Товар не выбран.", "Sale Page", MessageButton.OK, MessageIcon.Error);
                    SelectedOrderProduct = orderProduct;
                    ShowEditor(0);
                }
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Товары не выбраны.", "Sale Page", MessageButton.OK, MessageIcon.Error);
            }
        }

        private void Cleare()
        {
            _orderProducts.Clear();
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
