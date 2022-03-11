using RetailTrade.Domain.Models;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class EquipmentViewModel : BaseDialogViewModel
    {
        #region Private Members

        private int _selectedTypeEquipmentId;

        #endregion

        #region Public Properties

        public IEnumerable<TypeEquipment> TypeEquipments { get; set; }
        public int SelectedTypeEquipmentId
        {
            get => _selectedTypeEquipmentId;
            set
            {
                _selectedTypeEquipmentId = value;
                OnPropertyChanged(nameof(SelectedTypeEquipmentId));
            }
        }

        #endregion

        #region Commands



        #endregion

        #region Constructor

        public EquipmentViewModel()
        {

        }

        #endregion

        #region Private Voids



        #endregion
    }
}
