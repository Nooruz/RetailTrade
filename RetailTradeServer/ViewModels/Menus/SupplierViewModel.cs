using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using SalePageServer.State.Dialogs;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Menus
{
    public class SupplierViewModel : BaseViewModel
    {
        #region Private Members

        private readonly ISupplierService _supplierService;
        private readonly IDialogService _dialogService;
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



        #endregion

        #region Constructor

        public SupplierViewModel(ISupplierService supplierService,
            IDialogService dialogService)
        {
            _supplierService = supplierService;
            _dialogService = dialogService;

            CreateCommand = new RelayCommand(Create);
            EditCommand = new RelayCommand(Edit);

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
            //_dialogService.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _dialogService) { Title = "Поставщики (новый)" }, 
            //    new CreateSupplierProductDialogForm());
        }

        private void Edit()
        {
            //if (SelectedSupplier != null)
            //{
            //    _dialogService.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _dialogService) 
            //    { 
            //        Title = $"Поставщики ({SelectedSupplier.ShortName})",
            //        Id = SelectedSupplier.Id,
            //        FullName = SelectedSupplier.FullName,
            //        ShortName = SelectedSupplier.ShortName,
            //        Address = SelectedSupplier.Address,
            //        Phone = SelectedSupplier.Phone,
            //        Inn = SelectedSupplier.Inn,
            //        IsEditing = true
            //    },
            //    new CreateSupplierProductDialogForm());
            //}
            //else
            //{
            //    _dialogService.ShowMessage("Выберите поставщика.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //}
        }

        #endregion

        public override void Dispose()
        {
            Suppliers = null;
            SelectedSupplier = null;

            base.Dispose();
        }
    }
}
