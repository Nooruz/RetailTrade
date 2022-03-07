using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
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
        private readonly ITypeProductService _typeProductService;
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

        public ICommand LoadedCommand => new RelayCommand(GetArrivalsAsync);
        public ICommand CreateArrivalCommand => new RelayCommand(CreateArrival);
        public ICommand DeleteArrivalCommand => new RelayCommand(DeleteArrival);
        public ICommand DuplicateArrivalCommand => new RelayCommand(DuplicateArrival);
        public ICommand PrintArrivalCommand => new RelayCommand(PrintArrival);

        #endregion

        #region Constructor

        public ArrivalProductViewModel(IProductService productService,
            IArrivalService arrivalService,
            ISupplierService supplierService,
            ITypeProductService typeProductService)
        {
            _productService = productService;
            _arrivalService = arrivalService;
            _supplierService = supplierService;
            _typeProductService = typeProductService;

            Header = "Приход товара";

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

                DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel { PrintingDocument = report });
            }
            else
            {
                _ = MessageBox.Show("Выберите приход", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void GetArrivalsAsync()
        {
            Arrivals = await _arrivalService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        private void CreateArrival()
        {
            WindowService.Show(nameof(CreateArrivalProductDialogForm), new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _typeProductService) { Title = "Приход товаров (новый)" });
        }

        private async void DeleteArrival()
        {
            if (SelectedArrival != null)
            {
                if (MessageBox.Show("Вы точно хотите удалить выбранный приход?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _arrivalService.DeleteAsync(SelectedArrival.Id);
                }
            }
            else
            {
                MessageBox.Show("Выберите приход!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DuplicateArrival()
        {
            if (SelectedArrival != null)
            {
                if (MessageBox.Show("Дублировать выбранный приход?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    WindowService.Show(nameof(CreateArrivalProductDialogForm), 
                        new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _typeProductService) 
                    { 
                        Title = "Приход товаров (дублирование)",
                        SelectedSupplier = SelectedArrival.Supplier,
                        //ArrivalProducts = new(SelectedArrival.ArrivalProducts)
                    });
                }
            }
            else
            {
                MessageBox.Show("Выберите приход!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            SelectedArrival = null;
            Arrivals = null;
            _arrivalService.PropertiesChanged -= GetArrivalsAsync;
            base.Dispose();
        }

        #endregion        
    }
}
