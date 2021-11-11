using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Base;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels
{
    public class DialogWindowViewModel : BaseViewModel
    {
        #region Private Members

        private WindowState _windowState = WindowState.Normal;
        private ResizeMode _resizeMode = ResizeMode.NoResize;
        private SizeToContent _sizeToContent = SizeToContent.WidthAndHeight;

        #endregion

        #region Public Properties

        /// <summary>
        /// The title of this dialog window
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The content to host inside the dialog
        /// </summary>
        public UserControl Content { get; set; }
        public WindowState WindowState
        {
            get => _windowState;
            set => _windowState = value;
        }
        public ResizeMode ResizeMode
        {
            get => _resizeMode;
            set => _resizeMode = value;
        }
        public SizeToContent SizeToContent
        {
            get => _sizeToContent;
            set => _sizeToContent = value;
        }

        #endregion

        #region Commands

        private ICommand CloseCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogWindowViewModel(Window window)
        {
            CloseCommand = new RelayCommand(() => window.Close());
        }

        #endregion
    }
}
