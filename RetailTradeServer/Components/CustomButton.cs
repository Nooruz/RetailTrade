using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetailTradeServer.Components
{
    public class CustomButton : Button
    {
        #region Dependency Properties

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(CustomButton), new PropertyMetadata(""));

        public static readonly DependencyProperty HotKeyProperty =
            DependencyProperty.Register(nameof(HotKey), typeof(string), typeof(CustomButton), new PropertyMetadata(""));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(SolidColorBrush), typeof(CustomButton), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

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
