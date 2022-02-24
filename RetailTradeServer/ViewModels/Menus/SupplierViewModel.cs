using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.Views.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SupplierViewModel : BaseViewModel
    {
        #region Private Members

        private readonly ISupplierService _supplierService;
        private ObservableCollection<Supplier> _suppliers;

        #endregion

        #region Public Properties

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers ?? new();
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }

        public Supplier SelectedSupplier { get; set; }


        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public SupplierViewModel(ISupplierService supplierService)
        {
            _supplierService = supplierService;

            Header = "Поставщики";

            CreateCommand = new RelayCommand(Create);
            EditCommand = new RelayCommand(Edit);

            _supplierService.OnSupplierCreated += SupplierService_OnSupplierCreated;
        }

        #endregion

        #region Private Voids

        private async void UserControlLoaded()
        {
            Suppliers = new(await _supplierService.GetAllAsync());
            ShowLoadingPanel = false;
        }

        private void SupplierService_OnSupplierCreated(Supplier obj)
        {
            Suppliers.Add(obj);
        }

        private void Create()
        {
            WindowService.Show(nameof(CreateSupplierProductDialogForm), new CreateSupplierProductDialogFormModal(_supplierService) { Title = "Поставщики (новый)" });
        }

        private void Edit()
        {
            if (SelectedSupplier != null)
            {
                WindowService.Show(nameof(CreateSupplierProductDialogForm), new CreateSupplierProductDialogFormModal(_supplierService)
                {
                    Title = $"Поставщики ({SelectedSupplier.ShortName})",
                    Id = SelectedSupplier.Id,
                    FullName = SelectedSupplier.FullName,
                    ShortName = SelectedSupplier.ShortName,
                    Address = SelectedSupplier.Address,
                    Phone = SelectedSupplier.Phone,
                    Inn = SelectedSupplier.Inn,
                    IsEditing = true
                },
                new CreateSupplierProductDialogForm());
            }
            else
            {
                MessageBox.Show("Выберите поставщика.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion

        public override void Dispose()
        {
            Suppliers = null;
            SelectedSupplier = null;
            _supplierService.OnSupplierCreated -= SupplierService_OnSupplierCreated;
            base.Dispose();
        }
    }
}
