using RetailTrade.Domain.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RetailTradeClient.State.ProductSale
{
    public class ProductSaleStore : IProductSaleStore, INotifyPropertyChanged
    {
        #region Private Members

        private ObservableCollection<Sale> _productSales = new();
        private decimal _toBePaid;
        private decimal _entered;
        private bool _saleCompleted;

        #endregion

        #region Public Properties

        public ObservableCollection<Sale> ProductSales
        {
            get => _productSales;
            set
            {
                _productSales = value;
                OnPropertyChanged(nameof(ProductSales));
            }
        }
        public decimal ToBePaid
        {
            get => _toBePaid;
            set
            {
                _toBePaid = value;
                OnPropertyChanged(nameof(ToBePaid));
                OnPropertyChanged(nameof(Change));
            }
        }
        public decimal Entered
        {
            get => _entered;
            set
            {
                _entered = value;
                OnPropertyChanged(nameof(Entered));
                OnPropertyChanged(nameof(Change));
            }
        }
        public decimal Change => Entered - ToBePaid;
        public bool SaleCompleted
        {
            get => _saleCompleted;
            set
            {
                _saleCompleted = value;
                OnPropertyChanged(nameof(SaleCompleted));
            }
        }

        #endregion

        #region Constructor

        public ProductSaleStore()
        {
            _productSales = new();
        }

        #endregion

        #region Public Events

        public event Action OnPropertyChanged;
        
        #endregion

        #region Public Voids

        public void AddProduct(Sale sale)
        {
            ProductSales.Add(sale);
        }

        public void DeleteProduct(Sale sale)
        {
            _ = ProductSales.Remove(sale);
        }

        public void UpdateProduct(Sale sale)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
