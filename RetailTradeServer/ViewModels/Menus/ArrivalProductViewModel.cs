using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Barcode.Services;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ArrivalProductViewModel : BaseViewModel
    {
        #region Private members

        private readonly IProductService _productService;
        private readonly IArrivalService _arrivalService;
        private readonly ISupplierService _supplierService;
        private readonly ITypeProductService _typeProductService;
        private readonly IArrivalProductService _arrivalProductService;
        private readonly IBarcodeService _barcodeService;
        private Arrival _selectedArrival;
        private ObservableCollection<Arrival> _arrivals;

        #endregion

        #region Public properties

        public ObservableCollection<Arrival> Arrivals
        {
            get => _arrivals;
            set
            {
                _arrivals = value;
                OnPropertyChanged(nameof(Arrivals));
            }
        }
        public Arrival SelectedArrival
        {
            get => _selectedArrival;
            set
            {
                _selectedArrival = value;
                OnPropertyChanged(nameof(SelectedArrival));
            }
        }

        #endregion

        #region Commands

        public ICommand LoadedCommand => new RelayCommand(GetArrivalsAsync);

        #endregion

        #region Constructor

        public ArrivalProductViewModel(IProductService productService,
            IArrivalService arrivalService,
            ISupplierService supplierService,
            ITypeProductService typeProductService,
            IBarcodeService barcodeService,
            IArrivalProductService arrivalProductService)
        {
            _productService = productService;
            _arrivalService = arrivalService;
            _supplierService = supplierService;
            _typeProductService = typeProductService;
            _barcodeService = barcodeService;
            _arrivalProductService = arrivalProductService;

            Header = "Приход товара";

            _arrivalService.OnCreated += ArrivalService_OnCreated;
            _arrivalService.OnEdited += ArrivalService_OnEdited;
            _arrivalProductService.OnCreated += ArrivalProductService_OnCreated;
            _arrivalProductService.OnEdited += ArrivalProductService_OnEdited;
        }

        #endregion

        #region Private Voids

        private void ArrivalProductService_OnEdited(ArrivalProduct arrivalProduct)
        {
            try
            {
                Arrival arrival = Arrivals.FirstOrDefault(a => a.Id == arrivalProduct.ArrivalId);
                if (arrival != null)
                {
                    ArrivalProduct editArrivalProduct = arrival.ArrivalProducts.FirstOrDefault(a => a.Id == arrivalProduct.Id);
                    if (editArrivalProduct != null)
                    {
                        editArrivalProduct.Quantity = arrivalProduct.Quantity;
                        editArrivalProduct.SalePrice = arrivalProduct.SalePrice;
                        editArrivalProduct.ArrivalPrice = arrivalProduct.ArrivalPrice;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ArrivalProductService_OnCreated(ArrivalProduct arrivalProduct)
        {
            try
            {
                Arrival arrival = Arrivals.FirstOrDefault(a => a.Id == arrivalProduct.ArrivalId);
                if (arrival != null)
                {
                    arrival.ArrivalProducts.Add(arrivalProduct);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ArrivalService_OnEdited(Arrival arrival)
        {
            try
            {
                if (Arrivals.Any())
                {
                    Arrival editArrival = Arrivals.FirstOrDefault(a => a.Id == arrival.Id);
                    editArrival.InvoiceNumber = arrival.InvoiceNumber;
                    editArrival.InvoiceDate = arrival.InvoiceDate;
                    editArrival.Comment = arrival.Comment;
                    editArrival.Sum = arrival.Sum;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void ArrivalService_OnCreated(Arrival arrival)
        {
            try
            {
                if (Arrivals != null)
                {
                    Arrivals.Add(arrival);
                }
                else
                {
                    Arrivals = new();
                    Arrivals.Add(arrival);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void GetArrivalsAsync()
        {
            Arrivals = new(await _arrivalService.GetAllAsync());
            ShowLoadingPanel = false;
        }

        private bool CheckSelectedArrival()
        {
            if (SelectedArrival == null)
            {
                _ = MessageBoxService.ShowMessage("Выберите приход!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                return false;
            }
            return true;
        }

        #endregion

        #region Public Voids

        [Command]
        public void EditArrival()
        {
            if (CheckSelectedArrival())
            {
                try
                {
                    CreateArrivalProductDialogFormModel viewModel = new(_productService, _supplierService, _arrivalService, _typeProductService, _barcodeService, _arrivalProductService)
                    {
                        Title = $"Приход товаров №{SelectedArrival.Id} от {SelectedArrival.ArrivalDate}",
                        Arrival = SelectedArrival
                    };
                    WindowService.Show(nameof(CreateArrivalProductDialogForm), viewModel);
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        [Command]
        public void DuplicateArrival()
        {
            if (CheckSelectedArrival())
            {
                if (MessageBoxService.ShowMessage("Дублировать выбранный приход?", "", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    WindowService.Show(nameof(CreateArrivalProductDialogForm),
                        new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _typeProductService, _barcodeService, _arrivalProductService)
                        {
                            Title = "Приход товаров (дублирование)",
                            SelectedSupplierId = SelectedArrival.Supplier.Id,
                            //ArrivalProducts = new(SelectedArrival.ArrivalProducts)
                        });
                }
            }
        }

        [Command]
        public async void DeleteArrival()
        {
            if (CheckSelectedArrival())
            {
                if (MessageBoxService.ShowMessage("Вы точно хотите удалить выбранный приход?", "", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                {
                    await _arrivalService.DeleteAsync(SelectedArrival.Id);
                }
            }
        }

        [Command]
        public void CreateArrival()
        {
            WindowService.Show(nameof(CreateArrivalProductDialogForm), new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _typeProductService, _barcodeService, _arrivalProductService) { Title = "Приход товаров (новый)" });
        }

        [Command]
        public async void PrintArrival()
        {
            if (CheckSelectedArrival())
            {
                ArrivalProductReport report = new(new Arrival
                {
                    Id = SelectedArrival.Id,
                    Supplier = SelectedArrival.Supplier,
                    ArrivalDate = SelectedArrival.ArrivalDate
                });
                report.DataSource = SelectedArrival.ArrivalProducts;

                await report.CreateDocumentAsync();

                DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel { PrintingDocument = report });
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            SelectedArrival = null;
            Arrivals = null;
            _arrivalService.OnCreated -= ArrivalService_OnCreated;
            _arrivalService.OnEdited -= ArrivalService_OnEdited;
            base.Dispose();
        }

        #endregion        
    }
}
