using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.State.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class EditProductWithBarcodeDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IDataService<Unit> _unitService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;
        private readonly IMessageStore _messageStore;
        private int? _selectedUnitId;
        private int? _selectedSupplierId;
        private string _barcode;
        private string _name;
        private decimal _arrivalPrice;
        private decimal _salePrice;
        private string _tnved;
        private ObservableCollection<Unit> _units;
        private ObservableCollection<Supplier> _suppliers;
        private Product _editProduct;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public Product EditProduct
        {
            get => _editProduct;
            set
            {
                _editProduct = value;
                if (_editProduct != null)
                {
                    Name = _editProduct.Name;
                    ArrivalPrice = _editProduct.ArrivalPrice;
                    SalePrice = _editProduct.SalePrice;
                    TNVED = _editProduct.TNVED;
                    Barcode = _editProduct.Barcode;
                    SelectedSupplierId = _editProduct.SupplierId;
                    SelectedUnitId = _editProduct.UnitId;
                }
                OnPropertyChanged(nameof(EditProduct));
                OnPropertyChanged(nameof(BarcodeVisibility));
            }
        }
        public Visibility BarcodeVisibility => EditProduct.WithoutBarcode ? Visibility.Collapsed : Visibility.Visible;
        public ObservableCollection<Unit> Units
        {
            get => _units ?? (new());
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers ?? (new());
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }
        public int? SelectedUnitId
        {
            get => _selectedUnitId;
            set
            {
                _selectedUnitId = value;
                OnPropertyChanged(nameof(SelectedUnitId));
            }
        }
        public int? SelectedSupplierId
        {
            get => _selectedSupplierId;
            set
            {
                _selectedSupplierId = value;
                OnPropertyChanged(nameof(SelectedSupplierId));
            }
        }
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public decimal ArrivalPrice
        {
            get => _arrivalPrice;
            set
            {
                _arrivalPrice = value;
                OnPropertyChanged(nameof(ArrivalPrice));
            }
        }
        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                _salePrice = value;
                OnPropertyChanged(nameof(SalePrice));
            }
        }
        public string TNVED
        {
            get => _tnved;
            set
            {
                _tnved = value;
                OnPropertyChanged(nameof(TNVED));
            }
        }
        public Unit SelectedUnit { get; set; }
        public Supplier SelectedSupplier { get; set; }

        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CreateSupplierCommand { get; }

        #endregion

        #region Constructor

        public EditProductWithBarcodeDialogFormModel(IDataService<Unit> unitService,
            IProductService productService,
            ISupplierService supplierService,
            IDialogService dialogService,
            IMessageStore messageStore)
        {
            _unitService = unitService;
            _productService = productService;
            _supplierService = supplierService;
            _dialogService = dialogService;
            _messageStore = messageStore;
            GlobalMessageViewModel = new(_messageStore);

            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
            SaveCommand = new RelayCommand(Save);
            CreateSupplierCommand = new RelayCommand(CreateSupplier);

            _supplierService.OnSupplierCreated += SupplierService_OnSupplierCreated;
        }

        #endregion

        #region Private Voids

        private async void Save()
        {
            if (string.IsNullOrEmpty(Name))
            {
                _messageStore.SetCurrentMessage("Введите наименование товара.", MessageType.Error);
                return;
            }
            if (SelectedUnitId == null || SelectedUnitId == 0)
            {
                _messageStore.SetCurrentMessage("Выберите единицу измерения.", MessageType.Error);
                return;
            }
            if (ArrivalPrice <= 0)
            {
                _messageStore.SetCurrentMessage("Введите цену прихода товара.", MessageType.Error);
                return;
            }
            if (SalePrice <= 0)
            {
                _messageStore.SetCurrentMessage("Введите цену продажа товара.", MessageType.Error);
                return;
            }

            EditProduct.Name = Name;
            EditProduct.ArrivalPrice = ArrivalPrice;
            EditProduct.SalePrice = SalePrice;            
            EditProduct.TNVED = TNVED;
            EditProduct.Barcode = Barcode;
            EditProduct.SupplierId = SelectedSupplierId.Value;
            EditProduct.UnitId = SelectedUnitId.Value;
            EditProduct.WithoutBarcode = string.IsNullOrEmpty(Barcode);

            await _productService.UpdateAsync(EditProduct.Id, EditProduct);
            _dialogService.Close();
        }

        private async void UserControlLoaded()
        {
            Suppliers = new(await _supplierService.GetAllAsync());
            Units = new(await _unitService.GetAllAsync());
        }

        private void CreateSupplier()
        {
            //_dialogService.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _dialogService)
            //{
            //    Title = "Поставщик (создания)"
            //},
            //    new CreateSupplierProductDialogForm());
        }

        private void SupplierService_OnSupplierCreated(Supplier supplier)
        {
            Suppliers.Add(supplier);
            SelectedSupplier = supplier;
            SelectedSupplierId = supplier.Id;
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _supplierService.OnSupplierCreated -= SupplierService_OnSupplierCreated;
            base.Dispose();
        }

        #endregion
    }
}
