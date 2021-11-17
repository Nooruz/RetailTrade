using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
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
        private readonly IArrivalProductService _arrivalProductService;
        private readonly ISupplierService _supplierService;
        private readonly IUIManager _manager;
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

        #endregion

        #region Constructor

        public ArrivalProductViewModel(IProductService productService,
            IArrivalService arrivalService,
            IArrivalProductService arrivalProductService,
            ISupplierService supplierService,
            IUIManager manager)
        {
            _productService = productService;
            _arrivalService = arrivalService;
            _arrivalProductService = arrivalProductService;
            _supplierService = supplierService;
            _manager = manager;

            LoadedCommand = new RelayCommand(GetArrivalsAsync);
            CreateArrivalCommand = new RelayCommand(CreateArrival);
            DeleteArrivalCommand = new RelayCommand(DeleteArrival);
            DuplicateArrivalCommand = new RelayCommand(DuplicateArrival);

            _arrivalService.PropertiesChanged += GetArrivalsAsync;
        }        

        #endregion

        #region Private Voids

        private async void GetArrivalsAsync()
        {
            Arrivals = await _arrivalService.GetAllAsync();
        }

        private async void CreateArrival()
        {
            await _manager.ShowDialog(new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _manager) { Title = "Приход товаров (новый)" }, 
                new CreateArrivalProductDialogForm());
        }

        private async void DeleteArrival()
        {
            if (SelectedArrival != null)
            {
                if (_manager.ShowMessage("Вы точно хотите удалить выбранный приход?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _arrivalService.DeleteAsync(SelectedArrival.Id);
                }
            }
            else
            {
                _manager.ShowMessage("Выберите приход!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private async void DuplicateArrival()
        {
            if (SelectedArrival != null)
            {
                if (_manager.ShowMessage("Дублировать выбранный приход?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _manager.ShowDialog(
                        new CreateArrivalProductDialogFormModel(_productService, _supplierService, _arrivalService, _manager) 
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
                _manager.ShowMessage("Выберите приход!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
