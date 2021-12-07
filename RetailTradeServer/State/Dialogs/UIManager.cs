using RetailTradeServer.Commands;
using RetailTradeServer.Dialogs;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeServer.State.Dialogs
{
    public class UIManager : IUIManager
    {
        private DialogWindow _window;

        public MessageBoxResult ShowMessage(string message,
            string title, MessageBoxButton dialogButton, MessageBoxImage dialogIcon)
        {
            DialogWindow window = new();
            window.ViewModel = new(window);
            MessageBoxDialogViewModel viewModel = new(message, title, dialogButton, dialogIcon);
            window.Content = viewModel;
            window.Title = !string.IsNullOrEmpty(title) ? title : "Sale Page";
            viewModel.DialogWindow = window;
            _ = window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message,
            string title, MessageBoxButton dialogButton)
        {
            DialogWindow window = new();
            window.ViewModel = new(window);
            MessageBoxDialogViewModel viewModel = new(message, title, dialogButton);
            window.Content = viewModel;
            window.Title = title;
            viewModel.DialogWindow = window;
            _ = window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title)
        {
            DialogWindow window = new();
            window.ViewModel = new(window);
            MessageBoxDialogViewModel viewModel = new(message, title);
            window.Content = viewModel;
            window.Title = title;
            viewModel.DialogWindow = window;
            _ = window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public Task ShowDialog<TViewModel, TUserControl>(TViewModel viewModel, TUserControl userControl)
            where TViewModel : BaseDialogViewModel where TUserControl : BaseDialogUserControl
        {
            _window = userControl.Window;
            viewModel.CloseCommand = new RelayCommand(Close);
            return userControl.ShowDialog(viewModel);
        }

        public Task ShowDialog<TViewModel, TUserControl>(TViewModel viewModel,
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
            viewModel.CloseCommand = new RelayCommand(Close);
            return userControl.ShowDialog(viewModel);
        }

        public void Close()
        {
            _window?.Close();
        }
    }
}
