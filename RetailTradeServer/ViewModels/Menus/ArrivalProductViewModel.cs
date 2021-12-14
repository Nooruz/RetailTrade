using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ArrivalProductViewModel : BaseViewModel
    {
        #region Private members

        private readonly IProductService _productService;
        private readonly IArrivalService _arrivalService;
        private readonly ISupplierService _supplierService;
        private readonly IDialogService _dialogService;
        private Arrival _selectedArrival;
        private IEnumerable<Arrival> _arrivals;

        #endregion

        #region Public properties

        public IEnumerable<Arrival> Arrivals
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

        public ICommand LoadedCommand { get; }
        public ICommand CreateArrivalCommand { get; }
        public ICommand DeleteArrivalCommand { get; }
        public ICommand DuplicateArrivalCommand { get; }
        public ICommand PrintArrivalCommand { get; }

        #endregion

        #region Constructor

        public ArrivalProductViewModel(IProductService productService,
            IArrivalService arrivalService,
            ISupplierService supplierService,
            IDialogService dialogService)
        {
            _productService = productService;
            _arrivalService = arrivalService;
            _supplierService = supplierService;
            _dialogService = dialogService;

            LoadedCommand = new RelayCommand(GetArrivalsAsync);
            CreateArrivalCommand = new RelayCommand(CreateArrival);
            DeleteArrivalCommand = new RelayCommand(DeleteArrival);
            DuplicateArrivalCommand = new RelayCommand(DuplicateArrival);
            PrintArrivalCommand = new RelayCommand(PrintArrival);

            _arrivalService.PropertiesChanged += GetArrivalsAsync;
        }        

        #endregion

        #region Private Voids

        private async void PrintArrival()
        {
            if (SelectedArrival != null)
            {
                ArrivalProductReport report = new(new Arrival { Id = SelectedArrival.Id, 
                    Supplier = SelectedArrival.Supplier, ArrivalDate = SelectedArrival.ArrivalDate});
                report.DataSource = SelectedArrival.ArrivalProducts;

                await report.CreateDocumentAsync();

                await _dialogService.ShowDialog(new DocumentViewerViewModel() { PrintingDocument = report },
                    new DocumentViewerView());
            }
            else
            {
                _ = _dialogService.ShowMessage("Выберите приход", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void GetArrivalsAsync()
        {
            Arrivals = await _arrivalService.GetAllAsync();
        }

        private async void CreateArrival()
        {
            await _dialogService.ShowDialog(new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _dialogService) { Title = "Приход товаров (новый)" }, 
                new CreateArrivalProductDialogForm());
        }

        private async void DeleteArrival()
        {
            if (SelectedArrival != null)
            {
                if (_dialogService.ShowMessage("Вы точно хотите удалить выбранный приход?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _arrivalService.DeleteAsync(SelectedArrival.Id);
                }
            }
            else
            {
                _dialogService.ShowMessage("Выберите приход!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void DuplicateArrival()
        {
            if (SelectedArrival != null)
            {
                if (_dialogService.ShowMessage("Дублировать выбранный приход?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _dialogService.ShowDialog(
                        new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _dialogService) 
                        { 
                            Title = "Приход товаров (дублирование)",
                            SelectedSupplier = SelectedArrival.Supplier,
                            ArrivalProducts = new(SelectedArrival.ArrivalProducts)
                        },
                        new CreateArrivalProductDialogForm());
                }
            }
            else
            {
                _dialogService.ShowMessage("Выберите приход!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        public override void Dispose()
        {
            SelectedArrival = null;
            Arrivals = null;

            base.Dispose();
        }
    }
}
