using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetailTrade.DialogService
{
    public class Dialog : Control
    {
        #region Dependency

        public static readonly DependencyProperty FontAwesomeProperty =
            DependencyProperty.Register(nameof(FontAwesome), typeof(FontFamily), typeof(Dialog), new PropertyMetadata(new FontFamily()));

        #endregion        

        #region Public Properties

        public FontFamily FontAwesome
        {
            get => (FontFamily)GetValue(FontAwesomeProperty);
            set => SetValue(FontAwesomeProperty, value);
        }

        #endregion

        static Dialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Dialog), new FrameworkPropertyMetadata(typeof(Dialog)));
        }
    }
}
