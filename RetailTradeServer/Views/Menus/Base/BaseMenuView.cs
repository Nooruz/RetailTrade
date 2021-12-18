using DevExpress.Xpf.LayoutControl;
using System.Windows;
using System.Windows.Controls;

namespace RetailTradeServer.Views.Menus
{
    public class BaseMenuView : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ShowDefaultButtonsProperty =
            DependencyProperty.Register(nameof(ShowDefaultButtons), typeof(bool), typeof(BaseMenuView), new PropertyMetadata(true));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BaseMenuView), new PropertyMetadata("Название"));

        #endregion

        #region Public Properties

        public bool ShowDefaultButtons
        {
            get => (bool)GetValue(ShowDefaultButtonsProperty);
            set => SetValue(ShowDefaultButtonsProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion

        #region Constructor

        public BaseMenuView()
        {
            Style = FindResource("BaseUserControl") as Style;
        }

        #endregion
    }
}
