using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Authenticators;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Base
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;
    public delegate TMenuViewModel CreateMenuViewModel<TMenuViewModel>() where TMenuViewModel : BaseViewModel;
    public class BaseViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private bool _isModalOpen;
        private string _loadingPanelTitle = "Пожалуйста подаждите";
        private string _loadingPanelText = "Загрузка...";
        private bool _showLoadingPanel = true;
        private bool _allowHide = true;
        private bool _isSelected;
        private string _header;
        private WindowState _dialogState = WindowState.Normal;
        private Visibility _dialogVisibility = Visibility.Collapsed;
        private object _dialogContent;

        #endregion

        #region Commands

        public ICommand CreateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand DialogResizeCommand => new RelayCommand(() => DialogState = DialogState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
        public ICommand DialogCloseCommand => new RelayCommand(() => { DialogVisibility = Visibility.Collapsed; DialogContent = null; });

        #endregion

        #region Services

        protected IWindowService WindowService => GetService<IWindowService>();
        protected IDialogService DialogService => GetService<IDialogService>();
        protected ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();

        #endregion

        #region Public Properties

        public User CurrentUser => _authenticator.CurrentUser;
        public bool IsLogin => CurrentUser != null;
        public bool IsModalOpen
        {
            get => _isModalOpen;
            set
            {
                _isModalOpen = value;
                OnPropertyChanged(nameof(IsModalOpen));
            }
        }
        public string LoadingPanelTitle
        {
            get => _loadingPanelTitle;
            set
            {
                _loadingPanelTitle = value;
                OnPropertyChanged(nameof(LoadingPanelTitle));
            }
        }
        public string LoadingPanelText
        {
            get => _loadingPanelText;
            set
            {
                _loadingPanelText = value;
                OnPropertyChanged(nameof(LoadingPanelText));
            }
        }
        public bool ShowLoadingPanel
        {
            get => _showLoadingPanel;
            set
            {
                _showLoadingPanel = value;
                OnPropertyChanged(nameof(ShowLoadingPanel));
            }
        }        
        public bool AllowHide
        {
            get => _allowHide;
            set
            {
                _allowHide = value;
                OnPropertyChanged(nameof(AllowHide));
            }
        }
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }
        public Visibility DialogVisibility
        {
            get => _dialogVisibility;
            set
            {
                _dialogVisibility = value;
                OnPropertyChanged(nameof(DialogVisibility));
            }
        }
        public object DialogContent
        {
            get => _dialogContent;
            set
            {
                _dialogContent = value;
                OnPropertyChanged(nameof(DialogContent));
            }
        }
        public HorizontalAlignment DialogHorizontalAlignment => DialogState == WindowState.Normal ? HorizontalAlignment.Center : HorizontalAlignment.Stretch;
        public VerticalAlignment DialogVerticalAlignment => DialogState == WindowState.Normal ? VerticalAlignment.Center : VerticalAlignment.Stretch;
        public WindowState DialogState
        {
            get => _dialogState;
            set
            {
                _dialogState = value;
                OnPropertyChanged(nameof(DialogState));
                OnPropertyChanged(nameof(DialogHorizontalAlignment));
                OnPropertyChanged(nameof(DialogVerticalAlignment));
            }
        }

        #endregion

        #region Constructor

        public BaseViewModel(IAuthenticator authenticator)
        {
            _authenticator = authenticator;

            _authenticator.StateChanged += AuthenticatorStateChanged;
        }

        public BaseViewModel()
        {
            CloseCommand = new ParameterCommand(Close);
        }

        #endregion

        #region Public Voids не рабочий

        public void ShowDialog(BaseViewModel viewModel)
        {
            DialogContent = viewModel;
            DialogVisibility = Visibility.Visible;
        }

        public void CloseDialog()
        {
            DialogVisibility = Visibility.Collapsed;
            DialogContent = null;
        }

        #endregion

        #region Private Members

        private void Close(object parameter)
        {

        }

        private void AuthenticatorStateChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsLogin));
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Dispose()
        {
            if (_authenticator != null)
            {
                _authenticator.StateChanged -= AuthenticatorStateChanged;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion        
    }
}