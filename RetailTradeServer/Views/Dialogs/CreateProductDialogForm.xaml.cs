using System.Windows.Input;

namespace RetailTradeServer.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for ProductDialogForm.xaml
    /// </summary>
    public partial class CreateProductDialogForm : BaseDialogUserControl
    {
        public CreateProductDialogForm()
        {
            InitializeComponent();
        }

        private void BaseDialogUserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                KeyEventArgs tabPressEventArgs = new(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) { RoutedEvent = Keyboard.KeyDownEvent };
                InputManager.Current.ProcessInput(tabPressEventArgs);
            }
        }
    }
}
