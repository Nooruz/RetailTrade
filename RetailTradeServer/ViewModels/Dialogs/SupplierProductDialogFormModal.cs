using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System.Collections.Generic;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class SupplierProductDialogFormModal : BaseDialogViewModel
    {
        #region Private Members

        private readonly IUIManager _manager;
        private readonly ISupplierService _supplierService;

        #endregion

        #region Public Properties

        public IEnumerable<Supplier> Suppliers => _supplierService.GetAll();

        #endregion

        #region Commands

        public ICommand CreateSupplierProductCommand { get; }

        #endregion

        #region Constructor

        public SupplierProductDialogFormModal(IUIManager manager,
            ISupplierService supplierService)
        {
            _manager = manager;
            _supplierService = supplierService;

            CreateSupplierProductCommand = new RelayCommand(CreateSupplier);
            _supplierService.PropertiesChanged += SupplierService_PropertiesChanged;
        }

        #endregion

        #region Private Voids

        private void CreateSupplier()
        {
            _manager.ShowDialog(new CreateSupplierProductDialogFormModal(_supplierService, _manager)
            {
                Title = "Поставщики (новый)"
            },
                new CreateSupplierProductDialogForm());
        }

        private void SupplierService_PropertiesChanged()
        {
            OnPropertyChanged(nameof(Suppliers));
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            _supplierService.PropertiesChanged -= SupplierService_PropertiesChanged;
            base.Dispose();
        }

        #endregion
    }
}
