using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class LabelPrinting : INotifyPropertyChanged
    {
        private int _id;
        private int _unitId;
        private int _labelId;
        private string _name;
        private double _quantityInStock;
        private double _quantity;
        private string _barcode;
        private decimal _salePrice;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public int UnitId
        {
            get => _unitId;
            set
            {
                _unitId = value;
                OnPropertyChanged(nameof(UnitId));
            }
        }
        public int LabelId
        {
            get => _labelId;
            set
            {
                _labelId = value;
                OnPropertyChanged(nameof(LabelId));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public double QuantityInStock
        {
            get => _quantityInStock;
            set
            {
                _quantityInStock = value;
                OnPropertyChanged(nameof(QuantityInStock));
            }
        }
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }
        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                _salePrice = value;
                OnPropertyChanged(nameof(SalePrice));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
