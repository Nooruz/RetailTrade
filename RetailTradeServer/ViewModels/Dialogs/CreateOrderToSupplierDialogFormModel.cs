﻿using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Barcode;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateOrderToSupplierDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IProductService _productService;
        private readonly ITypeProductService _typeProductService;
        private readonly ISupplierService _supplierService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IZebraBarcodeScanner _zebraBarcodeScanner;
        private readonly IDataService<Unit> _unitService;
        private readonly IUserStore _userStore;
        private Supplier _selectedSupplier;
        private OrderProduct _selectedOrderProduct;
        private int _selectedOrderStatusId = 1;
        private string _comment;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Product> _products;
        private IEnumerable<OrderStatus> _orderStatuses;
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
        public IEnumerable<OrderStatus> OrderStatuses
        {
            get => _orderStatuses;
            set
            {
                _orderStatuses = value;
                OnPropertyChanged(nameof(OrderStatuses));
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
                GetProducts();
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
        public bool CanOrderProduct => OrderProducts.Any() && !OrderProducts.Any(p => p.Quantity == 0);
        public ProductDialogFormModel ProductDialogFormModel => new(_typeProductService) { Products = new(Products) };

        #endregion

        #region Commands

        public ICommand ValidateCellCommand => new ParameterCommand(parameter => ValidateCell(parameter));
        public ICommand OrderProductCommand => new RelayCommand(CreateOrder);
        public ICommand ClearCommand => new RelayCommand(Cleare);
        public ICommand AddProductToOrderCommand => new RelayCommand(AddProductToOrder);
        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);
        public ICommand CellValueChangedCommand => new ParameterCommand(p => CellValueChanged(p));
        public ICommand DeleteSelectedOrderProductCommand => new RelayCommand(DeleteSelectedOrderProduct);
        public ICommand ProductCommand => new RelayCommand(OpentProductDialog);

        #endregion

        #region Constructor

        public CreateOrderToSupplierDialogFormModel(IProductService productService,
            ITypeProductService typeProductService,
            ISupplierService supplierService,
            IOrderToSupplierService orderToSupplierService,
            IOrderStatusService orderStatusService,
            IZebraBarcodeScanner zebraBarcodeScanner,
            IDataService<Unit> unitService,
            IUserStore userStore)
        {
            _productService = productService;
            _typeProductService = typeProductService;
            _supplierService = supplierService;
            _orderToSupplierService = orderToSupplierService;
            _orderStatusService = orderStatusService;
            _unitService = unitService;
            _userStore = userStore;
            _zebraBarcodeScanner = zebraBarcodeScanner;

            CurrentWindowServiceCloseCommand = new RelayCommand(OnCurrentWindowClosing);

            _orderProducts = new();
        }

        #endregion

        #region Private Voids

        private void OnCurrentWindowClosing()
        {
            if (ProductDialogFormModel.SelectedProduct != null)
            {
                OrderProduct selectedOrderProduct = OrderProducts.FirstOrDefault(r => r.ProductId == ProductDialogFormModel.SelectedProduct.Id);
                if (selectedOrderProduct == null)
                {
                    selectedOrderProduct.ProductId = ProductDialogFormModel.SelectedProduct.Id;
                    selectedOrderProduct.Product = ProductDialogFormModel.SelectedProduct;
                    selectedOrderProduct.ArrivalPrice = ProductDialogFormModel.SelectedProduct.ArrivalPrice;
                }
                else
                {
                    _ = MessageBoxService.Show("Такой товар уже введен.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void OpentProductDialog()
        {
            WindowService.Show(nameof(ProductDialogForm), ProductDialogFormModel);
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
                    SelectedOrderProduct.ArrivalPrice = product.ArrivalPrice;
                    SelectedOrderProduct.Product = product;
                }
                TableView tableView = e.Source as TableView;
                tableView.PostEditor();
                tableView.Grid.UpdateTotalSummary();
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private async void UserControlLoaded()
        {
            Suppliers = await _supplierService.GetAllAsync();
            OrderStatuses = await _orderStatusService.GetAllAsync();
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
                        ArrivalPrice = product.ArrivalPrice,
                        Product = product
                    });
                }
            }
        }

        private void AddProductToOrder()
        {
            if (SelectedSupplier != null)
            {
                OrderProducts.Add(new OrderProduct());
            }
            else
            {
                _ = MessageBoxService.Show("Выберите поставщика!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private void ValidateCell(object parameter)
        {
            if (parameter is GridCellValidationEventArgs e)
            {
                if ((decimal)e.Value < 0)
                {
                    e.IsValid = false;
                    e.ErrorContent = "Количество заказа не должно быть 0.";
                    _ = MessageBoxService.Show("Количество заказа не должно быть 0.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            OnPropertyChanged(nameof(CanOrderProduct));
        }

        private async void CreateOrder()
        {
            OrderToSupplier order = await _orderToSupplierService.CreateAsync(new OrderToSupplier
            {
                OrderDate = DateTime.Now,
                SupplierId = SelectedSupplier.Id,
                OrderStatusId = 1,
                Comment = Comment,
                OrderProducts = OrderProducts.Select(o => new OrderProduct { ProductId = o.ProductId, Quantity = o.Quantity }).ToList()
            });

            OrderToSupplierReport report = new(order, SelectedSupplier, _userStore.CurrentOrganization)
            {
                DataSource = OrderProducts
            };

            await report.CreateDocumentAsync();

            DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel() { PrintingDocument = report });
        }

        private void Cleare()
        {
            _orderProducts.Clear();
        }

        private async void GetProducts()
        {
            if (SelectedSupplier != null)
            {
                Products = await _productService.PredicateSelect(p => p.SupplierId == SelectedSupplier.Id && p.DeleteMark == false, p => new Product { Id = p.Id, Name = p.Name, UnitId = p.UnitId, ArrivalPrice = p.ArrivalPrice, Barcode = p.Barcode });
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
