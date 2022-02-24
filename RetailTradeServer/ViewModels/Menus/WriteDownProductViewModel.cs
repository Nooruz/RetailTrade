using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.Report;
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

        public ICommand LoadedCommand => new RelayCommand(GetWriteDownsAsync);
        public ICommand DuplicateCommand { get; }
        public ICommand PrintCommand => new RelayCommand(Print);

        #endregion

        #region Constructor

        public WriteDownProductViewModel(IProductService productService,
            IWriteDownService writeDownService,
            ISupplierService supplierService)
        {
            _productService = productService;
            _writeDownService = writeDownService;
            _supplierService = supplierService;

            Header = "Списание товара";

            CreateCommand = new RelayCommand(Create);
            DeleteCommand = new RelayCommand(Delete);
            //DuplicateCommand = new RelayCommand(DuplicateArrival);

            _writeDownService.PropertiesChanged += GetWriteDownsAsync;
        }

        #endregion

        #region Private Voids

        private async void Print()
        {
            try
            {
                ShowLoadingPanel = true;
                if (SelectedWriteDown != null)
                {
                    WriteDownProductReport report = new(SelectedWriteDown.Id, SelectedWriteDown.WriteDownDate);
                    report.DataSource = SelectedWriteDown.WriteDownProducts;
                    await report.CreateDocumentAsync();
                    DocumentViewerService.Show(nameof(DocumentViewerView), new DocumentViewerViewModel() { PrintingDocument = report });
                }
                else
                {
                    MessageBox.Show("Выберите элемент.", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                }
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

        private void Create()
        {
            WindowService.Show(nameof(CreateWriteDownProductDialogForm), new CreateWriteDownProductDialogFormModel(_productService, _supplierService, _writeDownService) { Title = "Списание товаров (новый)" });
        }

        private async void Delete()
        {
            if (SelectedWriteDown != null)
            {
                if (MessageBox.Show("Вы точно хотите удалить?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await _writeDownService.DeleteAsync(SelectedWriteDown.Id);
                }
            }
            else
            {
                MessageBox.Show("Выберите элемента!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
