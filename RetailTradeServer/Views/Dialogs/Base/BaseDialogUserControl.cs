using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RetailTradeServer.Views.Dialogs
{
    public class BaseDialogUserControl : UserControl
    {
        #region Constructor

        public BaseDialogUserControl()
        {
            Interaction.GetBehaviors(this).Add(new WindowService()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                AllowSetWindowOwner = true,
                Name = "DialogService",
                WindowStyle = new Style
                {
                    TargetType = typeof(ThemedWindow),
                    BasedOn = FindResource("DialogService") as Style
                }
            });
            Interaction.GetBehaviors(this).Add(new WindowService()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                AllowSetWindowOwner = true,
                Name = "DocumentViewerService",
                WindowStyle = new Style
                {
                    TargetType = typeof(ThemedWindow),
                    BasedOn = FindResource("DocumentViewerService") as Style
                }
            });
            Interaction.GetBehaviors(this).Add(new DXMessageBoxService());
            Interaction.GetBehaviors(this).Add(new DialogService());
            Interaction.GetBehaviors(this).Add(new CurrentWindowService());
        }

        #endregion
    }
}
