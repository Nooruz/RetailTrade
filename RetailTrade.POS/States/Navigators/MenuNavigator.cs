using RetailTrade.POS.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RetailTrade.POS.States.Navigators
{
    public class MenuNavigator : IMenuNavigator
    {
        private BaseViewModel _currentViewModel;
        private ObservableCollection<BaseViewModel> _baseViewModels = new();

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();

                BaseViewModel baseViewModel = BaseViewModels.FirstOrDefault(v => v.ToString() == value.ToString());

                if (baseViewModel == null)
                {
                    _currentViewModel = value;
                    BaseViewModels.Add(value);
                }
                else
                {
                    _currentViewModel = baseViewModel;
                }

                StateChanged?.Invoke();
            }
        }

        public ObservableCollection<BaseViewModel> BaseViewModels
        {
            get => _baseViewModels;
            set => _baseViewModels = value;
        }

        public event Action? StateChanged;
    }
}
