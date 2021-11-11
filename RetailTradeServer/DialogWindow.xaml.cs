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
        private DialogWindowViewModel mViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// The view model for this window
        /// </summary>
        public DialogWindowViewModel ViewModel
        {
            get => mViewModel;
            set
            {
                // Set new value
                mViewModel = value;

                // Update data context
                DataContext = mViewModel;
            }
        }

        #endregion

        public DialogWindow()
        {
            InitializeComponent();
        }
    }
}
