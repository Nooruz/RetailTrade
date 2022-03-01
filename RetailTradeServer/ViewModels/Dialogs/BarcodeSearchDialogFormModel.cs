using DevExpress.Mvvm;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class BarcodeSearchDialogFormModel : BaseDialogViewModel
    {
        #region Public Properties

        public string Barcode { get; set; }
        public IEnumerable<UICommand> Commands { get; set; }

        #endregion

        #region Constructor

        public BarcodeSearchDialogFormModel()
        {
            Commands = new List<UICommand>()
            {
                new UICommand { Id = MessageBoxResult.OK, IsCancel = false, Caption = "Ок", IsDefault = true },
                new UICommand { Id = MessageBoxResult.Cancel, IsCancel = true, IsDefault = false, Caption = "Отмена" }
            };
        }

        #endregion
    }
}
