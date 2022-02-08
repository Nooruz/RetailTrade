using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.ObjectModel;

namespace RetailTradeServer.State.Navigators
{
    public class MenuNavigator : IMenuNavigator
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                if (CurrentViewModels == null)
                {
                    CurrentViewModels = new();
                }
                _currentViewModel.IsSelected = true;
                CurrentViewModels.Add(_currentViewModel);
                StateChanged?.Invoke(_currentViewModel);
            }
        }

        public ObservableCollection<BaseViewModel> CurrentViewModels { get; set; }

        public event Action<BaseViewModel> StateChanged;

        public void AddViewModel(BaseViewModel viewModel)
        {
            if (CurrentViewModels != null)
            {
                viewModel.IsSelected = true;
                CurrentViewModels.Add(viewModel);
            }
        }
    }
}
