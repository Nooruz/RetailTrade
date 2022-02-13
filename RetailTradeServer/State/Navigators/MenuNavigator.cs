using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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
                var v = CurrentViewModels.FirstOrDefault(v => v.ToString() == viewModel.ToString());
                if (v != null)
                {
                    v.IsSelected = true;
                }
                else
                {
                    viewModel.IsSelected = true;
                    CurrentViewModels.Add(viewModel);
                }                
            }
        }

        public void DeleteViewModel(BaseViewModel viewModel)
        {
            if (CurrentViewModels != null)
            {
                var v = CurrentViewModels.FirstOrDefault(v => v.ToString() == viewModel.ToString());
                if (v != null)
                {
                    CurrentViewModels.Remove(v);
                }                
            }
        }
    }
}
