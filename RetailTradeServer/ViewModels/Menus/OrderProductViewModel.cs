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
    public class OrderProductViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IOrderToSupplierService _orderToSupplierService;
        private readonly IUIManager _manager;
        

        #endregion

        #region Public Properties



        #endregion

        #region Commands

        

        #endregion

        #region Constructor

        public OrderProductViewModel(IOrderToSupplierService orderToSupplierService,
            IUIManager manager)
        {
            _orderToSupplierService = orderToSupplierService;
            _manager = manager;

        }

        #endregion

        #region Private Voids

        

        #endregion
    }
}
