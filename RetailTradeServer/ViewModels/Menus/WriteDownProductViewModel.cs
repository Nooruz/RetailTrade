using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System;
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
        private readonly ISupplierService _supplierService;
        private readonly IUIManager _manager;
        private WriteDown _selectedWriteDown;
        private IEnumerable<WriteDown> _writeDowns;
        private bool _showLoadingPanel;

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
        public bool ShowLoadingPanel
        {
            get => _showLoadingPanel;
            set
            {
                _showLoadingPanel = value;
                OnPropertyChanged(nameof(ShowLoadingPanel));
            }
        }

        #endregion

        #region Commands

        public ICommand LoadedCommand { get; }
        public ICommand DuplicateCommand { get; }
        public ICommand PrintCommand { get; }

        #endregion

        #region Constructor

        public WriteDownProductViewModel(IProductService productService,
            IWriteDownService writeDownService,
            ISupplierService supplierService,
            IUIManager manager)
        {
            _productService = productService;
            _writeDownService = writeDownService;
            _supplierService = supplierService;
            _manager = manager;

            LoadedCommand = new RelayCommand(GetWriteDownsAsync);
            CreateCommand = new RelayCommand(Create);
            DeleteCommand = new RelayCommand(Delete);
            PrintCommand = new RelayCommand(Print);
            //DuplicateCommand = new RelayCommand(DuplicateArrival);

            _writeDownService.PropertiesChanged += GetWriteDownsAsync;
        }

        #endregion

        #region Private Voids

        private void Print()
        {
            try
            {
                ShowLoadingPanel = true;
                //if (SelectedWriteDown != null)
                //{
                //    WriteDownProductReport report = new(SelectedWriteDown.Id, SelectedWriteDown.WriteDownDate);
                //    report.DataSource = SelectedWriteDown.WriteDownProducts;
                //    await report.CreateDocumentAsync();
                //    await _manager.ShowDialog(new DocumentViewerViewModel() { PrintingDocument = report },
                //        new DocumentViewerView(), WindowState.Maximized, ResizeMode.CanResize, SizeToContent.Manual);
                //}
                //else
                //{
                //    _manager.ShowMessage("Выберите элемент.", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                //}
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                //ShowLoadingPanel = false;
            }
        }

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

        public override void Dispose()
        {
            WriteDowns = null;
            SelectedWriteDown = null;

            base.Dispose();
        }
    }
}
