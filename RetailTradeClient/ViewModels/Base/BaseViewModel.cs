using DevExpress.Mvvm;
using RetailTradeClient.State.Users;
using System.ComponentModel;

namespace RetailTradeClient.ViewModels.Base
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;

    public class BaseViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Private Members

        private readonly IUserStore _userStore;

        #endregion

        #region Services

        protected IWindowService WindowService => GetService<IWindowService>("DialogService");
        protected IWindowService DocumentViewerService => GetService<IWindowService>("DocumentViewerService");
        protected IDialogService DialogService => GetService<IDialogService>();
        protected ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();
        protected IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

        #endregion        

        #region Constructor

        public BaseViewModel()
        {

        }

        #endregion

        #region Dispose

        public virtual void Dispose() { }

        #endregion

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
