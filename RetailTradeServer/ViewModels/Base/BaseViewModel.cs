using RetailTrade.Domain.Models;
using RetailTradeServer.State.Authenticators;
using System.ComponentModel;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Base
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;
    public delegate TMenuViewModel CreateMenuViewModel<TMenuViewModel>() where TMenuViewModel : BaseViewModel;
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Private Members

        private readonly IAuthenticator _authenticator;
        private bool _isModalOpen;

        #endregion

        public virtual void Dispose() { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Commands

        public ICommand CreateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }

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

        #endregion

        #region Constructor

        public BaseViewModel(IAuthenticator authenticator)
        {
            _authenticator = authenticator;

            _authenticator.StateChanged += AuthenticatorStateChanged;
        }

        public BaseViewModel()
        {

        }

        #endregion

        #region Private Members

        private void AuthenticatorStateChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsLogin));
        }

        #endregion
    }
}
