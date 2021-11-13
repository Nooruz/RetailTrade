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
    public class SupplierViewModel : BaseViewModel
    {
        #region Private Members

        private readonly ISupplierService _supplierService;
        private readonly IUIManager _manager;
        private IEnumerable<Supplier> _suppliers;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                OnPropertyChanged(nameof(Suppliers));
            }
        }

        public Supplier SelectedSupplier { get; set; }


        #endregion

        #region Commands

        public ICommand CreateCommand { get; }
        public ICommand UpdateCommand { get; }

        #endregion

        #region Constructor

        public SupplierViewModel(ISupplierService supplierService,
            IUIManager manager)
        {
            _supplierService = supplierService;
            _manager = manager;

            CreateCommand = new RelayCommand(Create);
            UpdateCommand = new RelayCommand(Update);

            GetSuppliers();

            _supplierService.PropertiesChanged += GetSuppliers;
        }

        #endregion

        #region Private Voids

        private async void GetSuppliers()
        {
            Suppliers = await _supplierService.GetAllAsync();
        }

        private void Create()
        {
            _manager.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _manager) { Title = "Поставщики (новый)" }, 
                new CreateSupplierProductDialogForm());
        }

        private void Update()
        {
            if (SelectedSupplier != null)
            {
                _manager.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _manager) 
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
                _manager.ShowMessage("Выберите поставщика.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
