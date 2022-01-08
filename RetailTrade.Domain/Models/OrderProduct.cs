﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class OrderProduct : DomainObject
    {
        #region Private Members

        private int _productId;
        private double _quantity;
        private Product _product;

        #endregion

        #region Public Properties

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
        /// Код заказа
        /// </summary>
        public int OrderToSupplierId { get; set; }

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
        /// Цена прихода
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrivalPrice { get; set; }

        /// <summary>
        /// Цена продажи
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

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

        /// <summary>
        /// Заказ
        /// </summary>
        public OrderToSupplier OrderToSupplier { get; set; }

        #endregion        
    }
}
