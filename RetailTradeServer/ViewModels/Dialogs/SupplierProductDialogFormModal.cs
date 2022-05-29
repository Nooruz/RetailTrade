using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class SupplierProductDialogFormModal : BaseDialogViewModel
    {
        #region Private Members

        private readonly ISupplierService _supplierService;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers => _supplierService.GetAll();

        #endregion

        #region Commands

        public ICommand CreateSupplierProductCommand { get; }

        #endregion

        #region Constructor

        public SupplierProductDialogFormModal(ISupplierService supplierService)
        {
            _supplierService = supplierService;

            CreateSupplierProductCommand = new RelayCommand(CreateSupplier);
        }

        #endregion

        #region Private Voids

        private void CreateSupplier()
        {
            //_dialogService.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _dialogService)
            //{
            //    Title = "Поставщики (новый)"
            //},
            //    new CreateSupplierProductDialogForm());
        }

        private void SupplierService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(Suppliers));
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion
    }
}
