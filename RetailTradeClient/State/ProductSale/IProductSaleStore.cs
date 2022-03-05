using RetailTrade.Domain.Models;
using System;
using System.Collections.ObjectModel;

namespace RetailTradeClient.State.ProductSale
{
    public interface IProductSaleStore
    {
        ObservableCollection<Sale> ProductSales { get; }
        decimal ToBePaid { get; set; }
        decimal Entered { get; set; }
        decimal Change { get; }
        public bool SaleCompleted { get; set; }
        event Action OnPropertyChanged;

        void AddProduct(Sale productSale);
        void DeleteProduct(Sale productSale);
        void UpdateProduct(Sale productSale);
    }
}
