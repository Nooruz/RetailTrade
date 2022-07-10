using DevExpress.Mvvm;
using System.ComponentModel;

namespace RetailTrade.POS.ViewModels
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;
    public delegate TMenuViewModel CreateMenuViewModel<TMenuViewModel>() where TMenuViewModel : BaseViewModel;
    public class BaseViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        protected IWindowService WindowService => GetService<IWindowService>("DialogService");
        protected IWindowService DocumentViewerService => GetService<IWindowService>("DocumentViewerService");
        protected IDialogService DialogService => GetService<IDialogService>();
        protected ICurrentWindowService CurrentWindowService => GetService<ICurrentWindowService>();
        protected IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;        

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public virtual void Dispose()
        {

        }
    }
}
