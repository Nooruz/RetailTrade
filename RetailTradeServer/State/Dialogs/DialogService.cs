using DevExpress.XtraReports.UI;
using RetailTradeServer;
using RetailTradeServer.Commands;
using RetailTradeServer.Dialogs;
using RetailTradeServer.ViewModels.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System.Diagnostics;
using System.Reflection;
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
        private DocumentViewerViewModel _documentViewerViewModel;
        private DocumentViewerView _documentViewerView;

        #endregion

        #region Constructor

        public DialogService()
        {
            _documentViewerViewModel = new();
            _documentViewerView = new();
        }

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
            TaskCompletionSource<bool> tcs = new();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    _window = new()
                    {
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        Padding = new Thickness(0),
                        BorderThickness = new Thickness(0),
                        Content = userControl
                    };
                    _window.Show();
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            });
            return tcs.Task;
        }

        public Task ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : BaseDialogViewModel
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    viewModel.CloseCommand = new RelayCommand(() => _window.Close());
                    _window = new()
                    {
                        MinHeight = WindowMinimumHeight,
                        MinWidth = WindowMinimumWidth,
                        SizeToContent = SizeToContent,
                        WindowStyle = WindowStyle,
                        ResizeMode = ResizeMode,
                        Content = viewModel,
                        Title = viewModel.Title ?? FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName
                    };                    
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
            MessageBoxDialogViewModel viewModel = new(message);
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode,
                ViewModel = new(_window),
                Content = viewModel,
            };
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title)
        {
            MessageBoxDialogViewModel viewModel = new(message, title);
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode,
                Title = string.IsNullOrEmpty(title) ? FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName : title,
                ViewModel = new(_window),
                Content = viewModel
            };
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton)
        {
            MessageBoxDialogViewModel viewModel = new(message, title, messageBoxButton);
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode,
                Title = string.IsNullOrEmpty(title) ? FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName : title,
                ViewModel = new(_window),
                Content = viewModel
            };
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            MessageBoxDialogViewModel viewModel = new(message, title, messageBoxButton, messageBoxImage);
            _window = new()
            {
                MinHeight = WindowMinimumHeight,
                MinWidth = WindowMinimumWidth,
                SizeToContent = SizeToContent,
                WindowStyle = WindowStyle,
                ResizeMode = ResizeMode,
                Title = string.IsNullOrEmpty(title) ? FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName : title,
                ViewModel = new(_window),
                Content = viewModel
            };
            viewModel.DialogWindow = _window;
            _ = _window.ShowDialog();
            return viewModel.MessageBoxResult;
        }

        public Task ShowPrintDialog(XtraReport report)
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    _documentViewerViewModel.PrintingDocument = report;
                    _documentViewerView.DataContext = _documentViewerViewModel;
                    _window = new()
                    {
                        MinHeight = WindowMinimumHeight,
                        MinWidth = WindowMinimumWidth,
                        WindowState = WindowState.Maximized,
                        Content = _documentViewerView,
                        Title = "Предварительный просмотр"
                    };
                    _window.ShowDialog();
                }
                finally
                {
                    tcs.TrySetResult(true);
                }
            });
            return tcs.Task;
        }
    }
}
