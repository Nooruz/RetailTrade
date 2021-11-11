using RetailTradeClient.Dialogs;
using RetailTradeClient.ViewModels.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.State.Dialogs
{
    public interface IUIManager
    {
        MessageBoxResult ShowMessage(string message,
            string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage);

        MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton);

        MessageBoxResult ShowMessage(string message, string title);

        Task<bool> ShowDialog<TViewModel, TUserControl>(TViewModel viewModel, TUserControl userControl)
            where TViewModel : BaseDialogViewModel where TUserControl : BaseDialogUserControl;

        Task<bool> ShowDialog<TViewModel, TUserControl>(TViewModel viewModel,
            TUserControl userControl,
            WindowState windowState,
            ResizeMode resizeMode,
            SizeToContent sizeToContent)
            where TViewModel : BaseDialogViewModel where TUserControl : BaseDialogUserControl;

        void Close();
    }
}
