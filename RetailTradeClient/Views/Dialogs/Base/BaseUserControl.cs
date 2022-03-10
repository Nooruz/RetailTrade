using System.Windows;
using System.Windows.Controls;

namespace RetailTradeClient.Views.Dialogs
{
    public class BaseUserControl : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BaseUserControl), new PropertyMetadata("Название"));

        public static readonly DependencyProperty ShowLoadingPanelProperty =
            DependencyProperty.Register(nameof(ShowLoadingPanel), typeof(bool), typeof(BaseUserControl), new PropertyMetadata(true));

        public static readonly DependencyProperty LoadingPanelTitleProperty =
            DependencyProperty.Register(nameof(LoadingPanelTitle), typeof(string), typeof(BaseUserControl), new PropertyMetadata("Пожалуйста подаждите"));

        public static readonly DependencyProperty LoadingPanelTextProperty =
            DependencyProperty.Register(nameof(LoadingPanelText), typeof(string), typeof(BaseUserControl), new PropertyMetadata("Загрузка..."));

        #endregion

        #region Public Properties

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public bool ShowLoadingPanel
        {
            get => (bool)GetValue(ShowLoadingPanelProperty);
            set => SetValue(ShowLoadingPanelProperty, value);
        }

        public bool LoadingPanelTitle
        {
            get => (bool)GetValue(LoadingPanelTitleProperty);
            set => SetValue(LoadingPanelTitleProperty, value);
        }

        public bool LoadingPanelText
        {
            get => (bool)GetValue(LoadingPanelTextProperty);
            set => SetValue(LoadingPanelTextProperty, value);
        }

        #endregion

        #region Constructor

        public BaseUserControl()
        {
            Style = FindResource("BaseUserControlStyle") as Style;
        }

        #endregion
    }
}
