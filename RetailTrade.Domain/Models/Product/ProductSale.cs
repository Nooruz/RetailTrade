﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class ProductSale : DomainObject
    {
        #region Private Members

        private double _quantity;
        private decimal _total;
        private decimal _discountAmount;
        private decimal _purchasePrice;
        private decimal _retailPrice;
        private int _productId;
        private int? _wareHouseId;
        private int? _pointSaleId;
        private int _receiptId;
        private Product _product;

        #endregion

        #region Public Properties

        /// <summary>
        /// Количество
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
        /// Итого
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        /// <summary>
        /// Сумма скидки
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                _discountAmount = value;
                OnPropertyChanged(nameof(DiscountAmount));
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
        /// Код товара
        /// </summary>
        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }

        /// <summary>
        /// Код склада
        /// </summary>
        public int? WareHouseId
        {
            get => _wareHouseId;
            set
            {
                _wareHouseId = value;
                OnPropertyChanged(nameof(WareHouseId));
            }
        }

        public int? PointSaleId
        {
            get => _pointSaleId;
            set
            {
                _pointSaleId = value;
                OnPropertyChanged(nameof(PointSaleId));
            }
        }

        /// <summary>
        /// Код чека
        /// </summary>
        public int ReceiptId
        {
            get => _receiptId;
            set
            {
                _receiptId = value;
                OnPropertyChanged(nameof(ReceiptId));
            }
        }

        /// <summary>
        /// Чек
        /// </summary>
        public Receipt Receipt { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public PointSale PointSale { get; set; }

        public WareHouse WareHouse { get; set; }

#endregion
    }
}
