using RetailTrade.Domain.Models;
using RetailTradeClient.Utilities;
using System;
using System.Collections.Generic;

namespace RetailTradeClient.State.ProductSale
{
    public class ProductSaleStore : IProductSaleStore
    {
        #region Private Members

        private ObservableQueue<Sale> _productSales;

        #endregion

        #region Public Properties

        public IEnumerable<Sale> ProductSales => _productSales;

        #endregion

        #region Constructor

        public ProductSaleStore()
        {
            _productSales = new();
        }

        #endregion

        #region Public Events

        public event Action<Sale> OnProductAdd;
        public event Action<Sale> OnProductDelete;
        public event Action<Sale> OnProductUpdate;

        #endregion

        #region Public Voids

        public void AddProduct(Sale Sale)
        {
            _productSales.Enqueue(Sale);
        }

        public void DeleteProduct(Sale Sale)
        {
            //_productSales.Dequeue();
        }

        public void Updateroduct(Sale Sale)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
