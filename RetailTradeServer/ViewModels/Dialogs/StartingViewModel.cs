using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Diagnostics;
using System.Reflection;

namespace SalePageServer.ViewModels.Dialogs
{
    public class StartingViewModel : BaseDialogViewModel
    {
        #region Public Properties

        public string ApplicationName => $"{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName} {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";

        #endregion
    }
}
