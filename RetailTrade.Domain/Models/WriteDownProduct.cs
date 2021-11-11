﻿using System;
using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Списание товаров
    /// </summary>
    public class WriteDownProduct : DomainObject, INotifyPropertyChanged
    {
        #region Private Members

        private int _productId;
        private decimal _quantity;
        private Product _product;

        #endregion

        #region Public Properties

        /// <summary>
        /// Дата списания товара
        /// </summary>
        public DateTime WriteDownDate { get; set; }

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
        /// Количество для списания
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

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
