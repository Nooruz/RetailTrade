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
    public class WriteDownProductViewModel : BaseViewModel
    {
        #region Private members

        private readonly IProductService _productService;
        private readonly IWriteDownService _writeDownService;
        private readonly IWriteDownProductService _writeDownProductService;
        private readonly ISupplierService _supplierService;
        private readonly IUIManager _manager;
        private WriteDown _selectedWriteDown;
        private IEnumerable<WriteDown> _writeDowns;

        #endregion

        #region Public properties

        public IEnumerable<WriteDown> WriteDowns
        {
            get => _writeDowns;
            set
            {
                _writeDowns = value;
                OnPropertyChanged(nameof(WriteDowns));
            }
        }
        public WriteDown SelectedWriteDown
        {
            get => _selectedWriteDown;
            set
            {
                _selectedWriteDown = value;
                OnPropertyChanged(nameof(SelectedWriteDown));
            }
        }

        #endregion

        #region Commands

        public ICommand LoadedCommand { get; }
        public ICommand DuplicateCommand { get; }

        #endregion

        #region Constructor

        public WriteDownProductViewModel(IProductService productService,
            IWriteDownService writeDownService,
            IWriteDownProductService writeDownProductService,
            ISupplierService supplierService,
            IUIManager manager)
        {
            _productService = productService;
            _writeDownService = writeDownService;
            _writeDownProductService = writeDownProductService;
            _supplierService = supplierService;
            _manager = manager;

            LoadedCommand = new RelayCommand(GetWriteDownsAsync);
            CreateCommand = new RelayCommand(Create);
            DeleteCommand = new RelayCommand(Delete);
            //DuplicateCommand = new RelayCommand(DuplicateArrival);

            _writeDownService.PropertiesChanged += GetWriteDownsAsync;
        }

        #endregion

        #region Private Voids

        private async void GetWriteDownsAsync()
        {
            WriteDowns = await _writeDownService.GetAllAsync();
        }

        private async void Create()
        {
            await _manager.ShowDialog(new CreateWriteDownProductDialogFormModel(_productService, _supplierService, _writeDownService, _manager) { Title = "Списание товаров (новый)" },
                new CreateWriteDownProductDialogForm());
        }

        private async void Delete()
        {
            if (SelectedWriteDown != null)
            {
                if (_manager.ShowMessage("Вы точно хотите удалить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _writeDownService.DeleteAsync(SelectedWriteDown.Id);
                }
            }
            else
            {
                _manager.ShowMessage("Выберите элемента!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
