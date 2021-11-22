using System.Collections.Generic;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Товары
    /// </summary>
    public class Product : DomainObject, INotifyPropertyChanged
    {
        #region Private Members

        private decimal _quantity;

        #endregion

        #region Public Properties

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Категория товара
        /// </summary>
        public int ProductSubcategoryId { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public int? SupplierId { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public int UnitId { get; set; }

        /// <summary>
        /// ТН ВЭД
        /// </summary>
        public string TNVED { get; set; }

        /// <summary>
        /// Штрих код
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Товар без штрих кода
        /// </summary>
        public bool WithoutBarcode { get; set; }

        /// <summary>
        /// Цена приход
        /// </summary>
        public decimal ArrivalPrice { get; set; }

        /// <summary>
        /// Количество на склада
        /// </summary>
        public decimal Quantity
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
        public decimal SalePrice { get; set; }

        public ProductSubcategory ProductSubcategory { get; set; }
        public Supplier Supplier { get; set; }
        public Unit Unit { get; set; }
        public ICollection<ProductSale> ProductSales { get; set; }
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
