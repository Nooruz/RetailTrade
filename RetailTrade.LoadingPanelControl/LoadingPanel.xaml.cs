using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace RetailTrade.LoadingPanelControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LoadingPanel : UserControl
    {
        #region Private Members

        private static BeginStoryboard _beginStoryboard;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(LoadingPanel), new PropertyMetadata(false, new PropertyChangedCallback(IsLoadingPropertyChanged)));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(LoadingPanel), new PropertyMetadata("Пожалуйста подождите"));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(LoadingPanel), new PropertyMetadata("Загузка..."));

        #endregion

        #region Public Properties

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region Constructor

        public LoadingPanel()
        {
            InitializeComponent();

            DataContext = this;

            _beginStoryboard = FindResource("LoadingPanelStory") as BeginStoryboard;
        }

        #endregion

        #region Private Voids

        private static Task RunStoryboard(BeginStoryboard story)
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    story.Storyboard.Begin();
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            });

            return tcs.Task;
        }

        private static Task StopStoryboard(BeginStoryboard story)
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    story.Storyboard.Stop();
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            });

            return tcs.Task;
        }

        private static void IsLoadingPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RunStoryboard(_beginStoryboard);
            }
            else
            {
                StopStoryboard(_beginStoryboard);
            }
        }

        #endregion        
    }
}