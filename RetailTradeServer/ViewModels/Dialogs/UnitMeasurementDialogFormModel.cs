using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class UnitMeasurementDialogFormModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IUnitService _unitService;
        private ObservableCollection<Unit> _units = new();
        private Unit _selectedUnit;

        #endregion

        #region Actions

        public event Action<Unit> OnSelected;

        #endregion

        #region Public Properties

        public ObservableCollection<Unit> Units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units));
            }
        }
        public Unit SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
            }
        }

        #endregion

        #region Constructor

        public UnitMeasurementDialogFormModel(IUnitService unitService)
        {
            Title = "Единицы измерения";
            _unitService = unitService;

            CreateCommand = new RelayCommand(Create);
            EditCommand = new RelayCommand(Edit);

            ViewModelLoaded();

            _unitService.OnCreated += UnitService_OnCreated;
        }

        #endregion

        #region Public Voids

        [Command]
        public void Select()
        {
            try
            {
                if (SelectedUnit != null)
                {
                    OnSelected?.Invoke(SelectedUnit);
                    CurrentWindowService.Close();
                }
                else
                {
                    _ = MessageBoxService.ShowMessage("Выберите единицу измерения!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Private Voids

        private void Edit()
        {
            try
            {
                WindowService.Show(nameof(CreateUnitDialogForm), new CreateUnitDialogFormModel(_unitService) { SelectedUnit = SelectedUnit, Title = $"Единица измерения ({SelectedUnit.ShortName})" });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void UnitService_OnCreated(Unit unit)
        {
            try
            {
                Units.Add(unit);
                SelectedUnit = unit;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void Create()
        {
            try
            {
                WindowService.Show(nameof(CreateUnitDialogForm), new CreateUnitDialogFormModel(_unitService) { Title = "Единица измерения (создание)" });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void ViewModelLoaded()
        {
            Units = new(await _unitService.GetAllAsync());
        }

        #endregion
    }
}
