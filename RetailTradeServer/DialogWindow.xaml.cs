using DevExpress.Xpf.Core;
using RetailTradeServer.ViewModels;

namespace RetailTradeServer
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : DXWindow
    {
        #region Private Members

        /// <summary>
        /// The view model for this window
        /// </summary>
        private DialogWindowViewModel _viewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The view model for this window
        /// </summary>
        public DialogWindowViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                // Set new value
                _viewModel = value;

                // Update data context
                DataContext = _viewModel;
            }
        }

        #endregion

        public DialogWindow()
        {
            InitializeComponent();
        }
    }
}
