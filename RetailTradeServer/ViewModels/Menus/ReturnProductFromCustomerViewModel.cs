using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ReturnProductFromCustomerViewModel : BaseViewModel
    {
        #region Private Members



        #endregion

        #region Public Properties



        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand => new RelayCommand(UserControlLoaded);

        #endregion

        #region Constructor

        public ReturnProductFromCustomerViewModel()
        {
            Header = "Возвраты товаров от клиентов";
        }

        #endregion

        #region Private Voids

        private void UserControlLoaded()
        {
            ShowLoadingPanel = false;
        }

        #endregion
    }
}
