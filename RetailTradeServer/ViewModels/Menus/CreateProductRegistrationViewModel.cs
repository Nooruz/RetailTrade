using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTradeServer.ViewModels.Base;

namespace RetailTradeServer.ViewModels.Menus
{
    public class CreateProductRegistrationViewModel : BaseViewModel
    {
        #region Private Members

        private Registration _registration = new();

        #endregion

        #region Public Properties

        public Registration CreatedRegistration
        {
            get => _registration;
            set
            {
                _registration = value;
                OnPropertyChanged(nameof(CreatedRegistration));
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public void UserControlLoaded()
        {
            Header = "Оприходование (создание)";
            ShowLoadingPanel = false;
        }

        #endregion
    }
}
