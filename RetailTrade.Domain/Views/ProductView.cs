using RetailTrade.Domain.Models;
using System.Collections.Generic;

namespace RetailTrade.Domain.Views
{
    public class ProductView : ViewObject
    {
        #region Private Members

        private string _name;
        private string _typeProduct;        
        private string _supplier;
        private string _unit;
        private string _tnved;
        private decimal _purchasePrice;
        private decimal _retailPrice;
        private bool _deleteMark;
        private string _description;
        private byte[] _image;
        private bool _prohibitDiscount;

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
        public string TypeProduct
        {
            get => _typeProduct;
            set
            {
                _typeProduct = value;
                OnPropertyChanged(nameof(TypeProduct));
            }
        }
        public string Supplier
        {
            get => _supplier;
            set
            {
                _supplier = value;
                OnPropertyChanged(nameof(Supplier));
            }
        }
        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }
        public string TNVED
        {
            get => _tnved;
            set
            {
                _tnved = value;
                OnPropertyChanged(nameof(TNVED));
            }
        }
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged(nameof(PurchasePrice));
            }
        }
        public decimal RetailPrice
        {
            get => _retailPrice;
            set
            {
                _retailPrice = value;
                OnPropertyChanged(nameof(RetailPrice));
            }
        }
        public bool DeleteMark
        {
            get => _deleteMark;
            set
            {
                _deleteMark = value;
                OnPropertyChanged(nameof(DeleteMark));
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public bool ProhibitDiscount
        {
            get => _prohibitDiscount;
            set
            {
                _prohibitDiscount = value;
                OnPropertyChanged(nameof(ProhibitDiscount));
            }
        }
        public ICollection<ProductBarcode> ProductBarcodes { get; set; }

        #endregion
    }
}
