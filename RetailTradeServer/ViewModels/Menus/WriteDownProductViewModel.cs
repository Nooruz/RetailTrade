using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using SalePageServer.State.Dialogs;
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
        private readonly IDialogService _dialogService;
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
            IDialogService dialogService)
        {
            _productService = productService;
            _writeDownService = writeDownService;
            _supplierService = supplierService;
            _dialogService = dialogService;

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
                //    await _dialogService.ShowDialog(new DocumentViewerViewModel() { PrintingDocument = report },
                //        new DocumentViewerView(), WindowState.Maximized, ResizeMode.CanResize, SizeToContent.Manual);
                //}
                //else
                //{
                //    _dialogService.ShowMessage("Выберите элемент.", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                //}
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                ShowLoadingPanel = false;
            }
        }

        private async void GetWriteDownsAsync()
        {
            WriteDowns = await _writeDownService.GetAllAsync();
            ShowLoadingPanel = false;
        }

        private async void Create()
        {
            await _dialogService.ShowDialog(new CreateWriteDownProductDialogFormModel(_productService, _supplierService, _writeDownService, _dialogService) { Title = "Списание товаров (новый)" });
        }

        private async void Delete()
        {
            if (SelectedWriteDown != null)
            {
                if (_dialogService.ShowMessage("Вы точно хотите удалить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _writeDownService.DeleteAsync(SelectedWriteDown.Id);
                }
            }
            else
            {
                _dialogService.ShowMessage("Выберите элемента!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        public override void Dispose()
        {
            WriteDowns = null;
            SelectedWriteDown = null;
            _writeDownService.PropertiesChanged -= GetWriteDownsAsync;
            base.Dispose();
        }
    }
}
