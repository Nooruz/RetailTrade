using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;

namespace RetailTradeClient.State.ProductSale
{
    public interface IProductSaleStore
    {
        IEnumerable<Sale> ProductSales { get; }
        event Action<Sale> OnProductAdd;
        event Action<Sale> OnProductDelete;
        event Action<Sale> OnProductUpdate;

        void AddProduct(Sale productSale);
        void DeleteProduct(Sale productSale);
        void Updateroduct(Sale productSale);
    }
}
