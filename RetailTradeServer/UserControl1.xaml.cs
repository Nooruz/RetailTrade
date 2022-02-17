using RetailTradeServer.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SalePageServer
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl, INotifyPropertyChanged
    {

        #region Private Members

        private WindowState _dialogState = WindowState.Normal;

        #endregion

        #region Public Properties

        public HorizontalAlignment DialogHorizontalAlignment => DialogState == WindowState.Normal ? HorizontalAlignment.Center : HorizontalAlignment.Stretch;
        public VerticalAlignment DialogVerticalAlignment => DialogState == WindowState.Normal ? VerticalAlignment.Center : VerticalAlignment.Stretch;
        public double DialogWidth => DialogState == WindowState.Normal ? 700 : double.NaN;
        public double DialogHeight => DialogState == WindowState.Normal ? 600 : double.NaN;
        public WindowState DialogState
        {
            get => _dialogState;
            set
            {
                _dialogState = value;
                OnPropertyChanged(nameof(DialogState));
                OnPropertyChanged(nameof(DialogHorizontalAlignment));
                OnPropertyChanged(nameof(DialogVerticalAlignment));
                OnPropertyChanged(nameof(DialogWidth));
                OnPropertyChanged(nameof(DialogHeight));
            }
        }

        #endregion

        #region Commands

        public ICommand DialogResizeCommand => new RelayCommand(() => DialogState = DialogState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);

        #endregion

        public UserControl1()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
