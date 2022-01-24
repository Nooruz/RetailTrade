using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Media;

namespace RetailTradeServer.Components
{
    public class CustomDropDownButton : DropDownButton
    {
        #region Dependency Properties

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached(nameof(Icon), typeof(string), typeof(CustomDropDownButton), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty HotKeyProperty =
            DependencyProperty.RegisterAttached(nameof(HotKey), typeof(string), typeof(CustomDropDownButton), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.RegisterAttached(nameof(IconColor), typeof(SolidColorBrush), typeof(CustomDropDownButton), new UIPropertyMetadata(new SolidColorBrush(Colors.Black)));

        #endregion

        #region Public Properties

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public string HotKey
        {
            get => (string)GetValue(HotKeyProperty);
            set => SetValue(HotKeyProperty, value);
        }
        public SolidColorBrush IconColor
        {
            get => (SolidColorBrush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        #endregion
    }
}
