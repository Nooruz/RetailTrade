using DevExpress.Xpf.WindowsUI;
using RetailTradeServer.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace RetailTradeServer.Components
{
    public class CustomHamburgerMenu : HamburgerMenu
    {
        #region Dependency Properties

        public static readonly DependencyProperty ContentsProperty =
            DependencyProperty.Register(nameof(Contents), typeof(ObservableCollection<BaseViewModel>), typeof(CustomHamburgerMenu), new PropertyMetadata());

        #endregion

        #region Public Properties

        public ObservableCollection<BaseViewModel> Contents
        {
            get => (ObservableCollection<BaseViewModel>)GetValue(ContentsProperty);
            set => SetValue(ContentsProperty, value);
        }

        #endregion        
    }
}
