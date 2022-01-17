namespace RetailTrade.Domain.Models
{
    public class ConnectedEquipment : DomainObject
    {
        #region Private Members

        private string _name;
        private int _typeEquipmentId;
        private string _serialNumber;

        #endregion

        #region Public Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        //public int TypeEquipmentId
        //{
        //    get => _typeEquipmentId;
        //    set
        //    {
        //        _typeEquipmentId = value;
        //        OnPropertyChanged(nameof(TypeEquipmentId));
        //    }
        //}
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged(nameof(SerialNumber));
            }
        }
        //public TypeEquipment TypeEquipment { get; set; }

        #endregion
    }
}
