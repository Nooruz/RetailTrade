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


        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(LoadingPanel), new PropertyMetadata(false));



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(LoadingPanel), new PropertyMetadata("Пожалуйста подождите"));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(LoadingPanel), new PropertyMetadata("Загузка..."));


        public LoadingPanel()
        {
            InitializeComponent();

            DataContext = this;
        }

        private async Task RunStoryboard(Storyboard story, FrameworkElement item)
        {
            story.Begin(item);
            while (story.GetCurrentState() == ClockState.Active && story.GetCurrentTime() == story.Duration)
            {
                await Task.Delay(100);
            }
        }
    }
}