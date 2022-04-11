using DevExpress.Xpf.Grid;
using RetailTradeClient.Views.Dialogs;
using System.Windows.Input;

namespace RetailTradeClient.Views
{
    /// <summary>
    /// Логика взаимодействия для HomeView.xaml
    /// </summary>
    public partial class HomeView : BaseUserControl
    {
        public HomeView()
        {
            InitializeComponent();

            RoutedCommand firstSettings = new RoutedCommand();
            firstSettings.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(firstSettings, CommandBinding_Executed));
        }

        private void TableView_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
