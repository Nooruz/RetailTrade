using RetailTradeServer;
using RetailTradeServer.Commands;
using RetailTradeServer.Dialogs;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Threading.Tasks;
using System.Windows;

namespace SalePageServer.State.Dialogs
{
    public class DialogService : IDialogService
    {
        #region Private Members

        private DialogWindow _window;
        private SizeToContent _sizeToContent = SizeToContent.WidthAndHeight;
        private WindowStyle _windowStyle = WindowStyle.ToolWindow;
        private ResizeMode _resizeMode = ResizeMode.NoResize;

        #endregion

        #region Public Properties

        public int WindowMinimumWidth { get; set; } = 250;
        public int WindowMinimumHeight { get; set; } = 100;
        public SizeToContent SizeToContent
        {
            get => _sizeToContent;
            set => _sizeToContent = value;
        }
        public WindowStyle WindowStyle
        {
            get => _windowStyle;
            set => _windowStyle = value;
        }
        public ResizeMode ResizeMode
        {
            get => _resizeMode;
            set => _resizeMode = value;
        }

        #endregion

        public void Close()
        {
            _window.Close();
        }

        public Task Show<TUserControl>(TUserControl userControl) where TUserControl : BaseDialogUserControl
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    _window = new()
                    {
                        MinHeight = WindowMinimumHeight,
                        MinWidth = WindowMinimumWidth,
                        SizeToContent = SizeToContent,
                        WindowStyle = WindowStyle,
                        ResizeMode = ResizeMode
                    };
                    _window.Content = userControl;
                    _window.Show();
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            });
            return tcs.Task; ;
        }

        public Task ShowDialog<TViewModel, TUserControl>(TViewModel viewModel, TUserControl userControl) where TUserControl : BaseDialogUserControl 
            where TViewModel : BaseDialogViewModel
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    _window = new()
                    {
                        MinHeight = WindowMinimumHeight,
                        MinWidth = WindowMinimumWidth,
                        SizeToContent = SizeToContent,
                        WindowStyle = WindowStyle,
                        ResizeMode = ResizeMode
                    };
                    viewModel.CloseCommand = new RelayCommand(() => _window.Close());
                    userControl.DataContext = viewModel;
                    _window.Content = userControl;
                    _window.Title = viewModel.Title;
                    _window.ShowDialog();
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            });
            return tcs.Task;
        }

        public MessageBoxResult ShowMessage(string message)
        {
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode
            };

            _window.ViewModel = new(_window);
            MessageBoxDialogViewModel viewModel = new(message);
            _window.Content = viewModel;
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();

            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title)
        {
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode
            };

            _window.ViewModel = new(_window);
            MessageBoxDialogViewModel viewModel = new(message, title);
            _window.Content = viewModel;
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();

            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton)
        {
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode
            };

            _window.ViewModel = new(_window);
            MessageBoxDialogViewModel viewModel = new(message, title, messageBoxButton);
            _window.Content = viewModel;
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();

            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode
            };

            _window.ViewModel = new(_window);
            MessageBoxDialogViewModel viewModel = new(message, title, messageBoxButton, messageBoxImage);
            _window.Content = viewModel;
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();

            return viewModel.MessageBoxResult;
        }
    }
}
