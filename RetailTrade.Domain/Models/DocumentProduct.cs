using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Models
{
    public class DocumentProduct : DomainObject
    {
        #region Private Members

        private int _productId;
        private int _documentId;
        private double _quantity;
        private double _stock;
        private double _stockTo;
        private decimal _price;
        private decimal _amount;
        private string _comment;

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
        /// Код документа
        /// </summary>
        public int DocumentId
        {
            get => _documentId;
            set
            {
                _documentId = value;
                OnPropertyChanged(nameof(DocumentId));
            }
        }

        /// <summary>
        /// Количество товара
        /// </summary>
        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                Amount = (decimal)Quantity * Price;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        /// <summary>
        /// Остаток, если перемещение то со склада
        /// </summary>
        public double Stock
        {
            get => _stock;
            set
            {
                _stock = value;
                OnPropertyChanged(nameof(Stock));
            }
        }

        /// <summary>
        /// Остаток на склад
        /// </summary>
        public double StockTo
        {
            get => _stockTo;
            set
            {
                _stockTo = value;
                OnPropertyChanged(nameof(StockTo));
            }
        }

        /// <summary>
        /// Цена товара
        /// </summary>
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                Amount = Price * (decimal)Quantity;
                OnPropertyChanged(nameof(Price));
            }
        }

        /// <summary>
        /// Сумма товара
        /// </summary>
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }

        public Document Document { get; set; }

        #endregion
    }
}
