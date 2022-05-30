using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateUnitDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IUnitService _unitService;
        private string _shortName;
        private string _longName;
        private Unit _selectedUnit;       

        #endregion

        #region Public Properties

        public string ShortName
        {
            get => _shortName;
            set
            {
                _shortName = value;
                OnPropertyChanged(nameof(ShortName));
            }
        }
        public string LongName
        {
            get => _longName;
            set
            {
                _longName = value;
                OnPropertyChanged(nameof(LongName));
            }
        }
        public Unit SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                _selectedUnit = value;
                if (_selectedUnit != null)
                {
                    ShortName = _selectedUnit.ShortName;
                    LongName = _selectedUnit.LongName;
                    IsEditMode = true;
                }
                OnPropertyChanged(nameof(SelectedUnit));
                OnPropertyChanged(nameof(IsEditMode));
            }
        }
        public bool IsEditMode { get; set; }

        #endregion

        #region Constructor

        public CreateUnitDialogFormModel(IUnitService unitService)
        {
            CreateCommand = new RelayCommand(Create);
            _unitService = unitService;
        }

        #endregion

        #region Private Voids

        private async void Create()
        {
            if (IsEditMode)
            {
                SelectedUnit.ShortName = ShortName;
                SelectedUnit.LongName = LongName;
                _ = await _unitService.UpdateAsync(SelectedUnit.Id, SelectedUnit);
                CurrentWindowService.Close();
            }
            else
            {
                _ = await _unitService.CreateAsync(new Unit
                {
                    ShortName = ShortName,
                    LongName = LongName
                });
                CurrentWindowService.Close();
            }
        }

        #endregion
    }
}
