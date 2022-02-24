using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using System.Windows.Controls;

namespace RetailTradeServer.Dialogs
{
    public class BaseDialogUserControl : UserControl
    {
        #region Constructor

        public BaseDialogUserControl()
        {
            Interaction.GetBehaviors(this).Add(new CurrentWindowService());
        }

        #endregion
    }
}
