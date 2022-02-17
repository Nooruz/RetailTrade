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

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register(nameof(DialogContent), typeof(object), typeof(Dialog), new PropertyMetadata(null));

        #endregion

        #region Public Properties

        public FontFamily FontAwesome
        {
            get => (FontFamily)GetValue(FontAwesomeProperty);
            set => SetValue(FontAwesomeProperty, value);
        }

        public object DialogContent
        {
            get => GetValue(DialogContentProperty);
            set => SetValue(DialogContentProperty, value);
        }

        #endregion

        static Dialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Dialog), new FrameworkPropertyMetadata(typeof(Dialog)));            
        }
    }
}
