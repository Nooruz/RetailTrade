using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core;
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

        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.Register(nameof(Buttons), typeof(UIElementCollection), typeof(BaseMenuView), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowLoadingPanelProperty =
            DependencyProperty.Register(nameof(ShowLoadingPanel), typeof(bool), typeof(BaseMenuView), new PropertyMetadata(true));

        public static readonly DependencyProperty LoadingPanelTitleProperty =
            DependencyProperty.Register(nameof(LoadingPanelTitle), typeof(string), typeof(BaseMenuView), new PropertyMetadata("Пожалуйста подаждите"));

        public static readonly DependencyProperty LoadingPanelTextProperty =
            DependencyProperty.Register(nameof(LoadingPanelText), typeof(string), typeof(BaseMenuView), new PropertyMetadata("Загрузка..."));

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

        public UIElementCollection Buttons
        {
            get { return (UIElementCollection)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
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

        public BaseMenuView()
        {
            Interaction.GetBehaviors(this).Add(new WindowService()
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                AllowSetWindowOwner = true,
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
            Interaction.GetBehaviors(this).Add(new DialogService());
            Style = FindResource("BaseUserControl") as Style;
        }

        #endregion
    }
}
