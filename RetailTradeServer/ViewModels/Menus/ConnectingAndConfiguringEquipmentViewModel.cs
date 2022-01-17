using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class ConnectingAndConfiguringEquipmentViewModel : BaseViewModel
    {
        #region Private Members



        #endregion

        #region Public Properties



        #endregion

        #region Commands

        public ICommand UserControlLoadedCommand { get; }

        #endregion

        #region Constructor

        public ConnectingAndConfiguringEquipmentViewModel()
        {
            UserControlLoadedCommand = new RelayCommand(UserControlLoaded);
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
