using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.ViewModels.Menus
{
    public class PriceListViewModel : BaseViewModel
    {
        #region Private Members



        #endregion

        #region Public Properties



        #endregion

        #region Constructor

        public PriceListViewModel()
        {
            Header = "Цены (прайс-лист)";

            ViewModelLoaded();
        }

        #endregion

        #region Private Voids

        private void ViewModelLoaded()
        {
            ShowLoadingPanel = false;
        }

        #endregion

        #region Public Voids



        #endregion
    }
}
