using RetailTradeClient.State.Users;
using System.ComponentModel;

namespace RetailTradeClient.ViewModels.Base
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;

    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Private Members

        private readonly IUserStore _userStore;

        #endregion

        public virtual void Dispose() { }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Constructor

        public BaseViewModel()
        {

        }

        #endregion
    }
}
