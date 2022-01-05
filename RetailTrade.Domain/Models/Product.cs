using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Товары
    /// </summary>
    public class Product : DomainObject, INotifyPropertyChanged
    {
        #region Private Members

        private string _name;
        private int _productCategoryId;
        private int _productSubcategoryId;
        private int? _supplierId;
        private int _unitId;
        private string _TNVED;
        private string _barcode;
        private double _quantity;
        private bool _withoutBarcode;
        private decimal _arrivalPrice;
        private decimal _salePrice;
        //private ProductCategory _productCategory;
        private ProductSubcategory _productSubcategory;
        private Unit _unit;
        private bool _deleteMark;

        #endregion

        #region Public Properties

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        /// Категория товара
        /// </summary>
        public int ProductCategoryId
        {
            get => _productCategoryId;
            set
            {
                _productCategoryId = value;
                OnPropertyChanged(nameof(ProductCategoryId));
            }
        }

        /// <summary>
        /// Группа товара
        /// </summary>
        public int ProductSubcategoryId
        {
            get => _productSubcategoryId;
            set
            {
                _productSubcategoryId = value;
                OnPropertyChanged(nameof(ProductSubcategoryId));
            }
        }

        /// <summary>
        /// Поставщик
        /// </summary>
        public int? SupplierId
        {
            get => _supplierId;
            set
            {
                _supplierId = value;
                OnPropertyChanged(nameof(SupplierId));
            }
        }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public int UnitId
        {
            get => _unitId;
            set
            {
                _unitId = value;
                OnPropertyChanged(nameof(UnitId));
            }
        }

        /// <summary>
        /// ТН ВЭД
        /// </summary>
        public string TNVED
        {
            get => _TNVED;
            set
            {
                _TNVED = value;
                OnPropertyChanged(nameof(TNVED));
            }
        }

        /// <summary>
        /// Штрих код
        /// </summary>
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                OnPropertyChanged(nameof(Barcode));
            }
        }

        /// <summary>
        /// Товар без штрих кода
        /// </summary>
        public bool WithoutBarcode
        {
            get => _withoutBarcode;
            set
            {
                _withoutBarcode = value;
                OnPropertyChanged(nameof(WithoutBarcode));
            }
        }

        /// <summary>
        /// Цена приход
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrivalPrice
        {
            get => _arrivalPrice;
            set
            {
                _arrivalPrice = value;
                OnPropertyChanged(nameof(ArrivalPrice));
            }
        }

        /// <summary>
        /// Количество на склада
        /// </summary>
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        /// <summary>
        /// Цена продажа
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                _salePrice = value;
                OnPropertyChanged(nameof(SalePrice));
            }
        }

        /// <summary>
        /// Пометка на удаление по умолчанию false
        /// </summary>
        public bool DeleteMark
        {
            get => _deleteMark;
            set
            {
                _deleteMark = value;
                OnPropertyChanged(nameof(DeleteMark));
            }
        }

        public ProductSubcategory ProductSubcategory
        {
            get => _productSubcategory;
            set
            {
                _productSubcategory = value;
                OnPropertyChanged(nameof(ProductSubcategory));
            }
        }
        //public ProductCategory ProductCategory
        //{
        //    get => _productCategory;
        //    set
        //    {
        //        _productCategory = value;
        //        OnPropertyChanged(nameof(ProductCategory));
        //    }
        //}
        public Supplier Supplier { get; set; }
        public Unit Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }
        public ICollection<ProductSale> ProductSales { get; set; }
        public ICollection<ProductRefund> ProductRefunds { get; set; }
        public ICollection<ArrivalProduct> ArrivalProducts { get; set; }
        public ICollection<WriteDownProduct> WriteDownProducts { get; set; }
        public ICollection<RefundToSupplierProduct> ProductRefundToSuppliers { get; set; }
        public ICollection<OrderProduct> Orders { get; set; }

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
