using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Factories;
using RetailTradeServer.Views.Dialogs;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IUIManager _manager;
        private readonly IShiftService _shiftService;

        #endregion

        #region Public Properties

        public BaseViewModel CurrentMenuViewModel => _menuNavigator.CurrentViewModel;

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand { get; }        

        #region Товары

        public ICommand ProductCategoryCommand { get; }
        public ICommand ArrivalProductCommand { get; }
        public ICommand WriteDownProductCommand { get; }
        public ICommand OrderProductCommand { get; }
        public ICommand RefundToSupplierCommand { get; }

        #endregion

        #region Справочники и инструменты

        public ICommand BarcodeCommand { get; }
        public ICommand UserCommand { get; }
        public ICommand BranchCommand { get; }

        #endregion

        #region Отчеты

        public ICommand ReportClosingShiftsCommand { get; }

        #endregion

        #endregion

        #region Constructor

        public HomeViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IUIManager manager,
            IShiftService shiftService)
        {
            _menuNavigator = menuNavigator;
            _manager = manager;
            _shiftService = shiftService;

            UpdateCurrentMenuViewModelCommand = new UpdateCurrentMenuViewModelCommand(_menuNavigator, menuViewModelFactory);

            ProductCategoryCommand = new RelayCommand(ProductCategory);
            ArrivalProductCommand = new RelayCommand(ArrivalProduct);
            WriteDownProductCommand = new RelayCommand(WriteDownProduct);
            OrderProductCommand = new RelayCommand(OrderProduct);
            BarcodeCommand = new RelayCommand(Barcode);
            UserCommand = new RelayCommand(User);
            BranchCommand = new RelayCommand(Branch);
            ReportClosingShiftsCommand = new RelayCommand(ReportClosingShifts);
            RefundToSupplierCommand = new RelayCommand(RefundToSupplier);

            _menuNavigator.StateChanged += MenuNavigator_StateChanged;
        }

        #endregion

        #region Private Voids

        private void ProductCategory()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ProductCategory);
        }

        private void RefundToSupplier()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.RefundToSupplier);
        }

        private void ArrivalProduct()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ArrivalProduct);
        }

        private void WriteDownProduct()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.WriteDownProduct);
        }

        private void OrderProduct()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.OrderProduct);
        }

        private void User()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.User);
        }

        private void Branch()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.Branch);
        }

        private void MenuNavigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentMenuViewModel));
        }

        /// <summary>
        /// Ценники и этикетки (Штрих-код)
        /// </summary>
        private void Barcode()
        {
            UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.ProductBarcode);
        }

        private void ReportClosingShifts()
        {
            _manager.ShowDialog(new ReportClosingShiftsDialogFormModel(_manager, _shiftService), new ReportClosingShiftsDialogForm());
        }

        #endregion
    }
}
