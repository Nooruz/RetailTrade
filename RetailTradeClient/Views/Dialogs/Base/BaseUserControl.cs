using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core;
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
