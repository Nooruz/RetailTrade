using System.ComponentModel;

namespace RetailTrade.Domain.Models
{
    public class Sale : INotifyPropertyChanged
    {
        #region Private Members

        private double _quantity;

        #endregion

        #region Public Properties

        /// <summary>
        /// Код товара
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество покупки
        /// </summary>
        public double Quantity
        {
            get => _quantity;
            set
            {                
                if (value > 0)
                {
                    _quantity = value;
                    Sum = SalePrice * (decimal)_quantity;
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }

        /// <summary>
        /// Цена продажи
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// Цена прихода
        /// </summary>
        public decimal ArrivalPrice { get; set; }

        /// <summary>
        /// Сумма покупки
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Количество на складе
        /// </summary>
        public double QuantityInStock { get; set; }

        public string TNVED { get; set; }

        #endregion        

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
