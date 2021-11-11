using RetailTradeClient.Dialogs;
using RetailTradeClient.ViewModels;
using RetailTradeClient.ViewModels.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.State.Dialogs
{
    public class UIManager : IUIManager
    {
        private DialogWindow _window;
        public MessageBoxResult ShowMessage(string message,
            string title, MessageBoxButton dialogButton, MessageBoxImage dialogIcon)
        {
            DialogWindow window = new();
            window.ViewModel = new DialogWindowViewModel(window);
            MessageBoxDialogViewModel viewModel = new(message, title, dialogButton, dialogIcon);
            window.Content = viewModel;
            window.Title = !string.IsNullOrEmpty(title) ? title : "Управление розничной торговлей";
            viewModel.DialogWindow = window;
            window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message,
            string title, MessageBoxButton dialogButton)
        {
            DialogWindow window = new();
            window.ViewModel = new DialogWindowViewModel(window);
            MessageBoxDialogViewModel viewModel = new(message, title, dialogButton);
            window.Content = viewModel;
            window.Title = title;
            viewModel.DialogWindow = window;
            window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title)
        {
            DialogWindow window = new DialogWindow();
            window.ViewModel = new DialogWindowViewModel(window);
            MessageBoxDialogViewModel viewModel = new MessageBoxDialogViewModel(message, title);
            window.Content = viewModel;
            window.Title = title;
            viewModel.DialogWindow = window;
            window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public Task<bool> ShowDialog<TViewModel, TUserControl>(TViewModel viewModel, TUserControl userControl)
            where TViewModel : BaseDialogViewModel where TUserControl : BaseDialogUserControl
        {
            _window = userControl.Window;
            return userControl.ShowDialog(viewModel);
        }

        public Task<bool> ShowDialog<TViewModel, TUserControl>(TViewModel viewModel,
            TUserControl userControl,
            WindowState windowState,
            ResizeMode resizeMode,
            SizeToContent sizeToContent)
            where TViewModel : BaseDialogViewModel where TUserControl : BaseDialogUserControl
        {
            _window = userControl.Window;
            _window.ViewModel.WindowState = windowState;
            _window.ViewModel.ResizeMode = resizeMode;
            _window.ViewModel.SizeToContent = sizeToContent;
            return userControl.ShowDialog(viewModel);
        }

        public void Close()
        {
            _window?.Close();
        }
    }
}
