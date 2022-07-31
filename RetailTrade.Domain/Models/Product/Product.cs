﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Товары
    /// </summary>
    public class Product : DomainObject
    {
        #region Private Members

        private string _name;
        private int _typeProductId;
        private int? _supplierId;
        private int _unitId;
        private string _TNVED;
        private string _barcode;
        private bool _withoutBarcode;
        private decimal _purchasePrice;
        private decimal _retailPrice;
        private bool _deleteMark;
        private Unit _unit;
        private byte[] _image;
        private string _description;
        private bool _prohibitDiscount;
        private ObservableCollection<ProductBarcode> _productBarcodes = new();

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
        /// Вид товара
        /// </summary>
        public int TypeProductId
        {
            get => _typeProductId;
            set
            {
                _typeProductId = value;
                OnPropertyChanged(nameof(TypeProductId));
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
        /// Закупочная цена
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = value;
                OnPropertyChanged(nameof(PurchasePrice));
            }
        }

        /// <summary>
        /// Розничная цена
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailPrice
        {
            get => _retailPrice;
            set
            {
                _retailPrice = value;
                OnPropertyChanged(nameof(RetailPrice));
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

        /// <summary>
        /// Рисунок продукта
        /// </summary>
        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// Запретить скидки
        /// </summary>
        public bool ProhibitDiscount
        {
            get => _prohibitDiscount;
            set
            {
                _prohibitDiscount = value;
                OnPropertyChanged(nameof(ProhibitDiscount));
            }
        }

        public Unit Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }
        public Supplier Supplier { get; set; }        
        public TypeProduct TypeProduct { get; set; }
        public ICollection<ProductSale> ProductSales { get; set; }
        public ICollection<ProductRefund> ProductRefunds { get; set; }
        public ICollection<ArrivalProduct> ArrivalProducts { get; set; }
        public ICollection<WriteDownProduct> WriteDownProducts { get; set; }
        public ICollection<RefundToSupplierProduct> ProductRefundToSuppliers { get; set; }
        public ICollection<OrderProduct> Orders { get; set; }
        public ICollection<WareHouse> WareHouses { get; set; }
        public ICollection<RevaluationProduct> RevaluationProducts { get; set; }
        public ICollection<RegistrationProduct> RegistrationProducts { get; set; }
        public ObservableCollection<ProductBarcode> ProductBarcodes
        {
            get => _productBarcodes;
            set
            {
                _productBarcodes = value;
                OnPropertyChanged(nameof(ProductBarcodes));
            }
        }

        #endregion
    }
}
