using RetailTrade.Domain.Models;
using RetailTrade.Domain.Views;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductService : IDataService<Product>
    {
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetByBarcodeAsync(string barcode);
        IEnumerable<Product> GetForRefund(int supplierId);
        Task<IEnumerable<Product>> PredicateSelect(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
        Task<Product> Predicate(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
        Task<double> GetQuantity(int id);
        Task<double> Refund(int id, double quantity);
        Task<bool> Refunds(IEnumerable<ProductRefund> productRefunds);
        Task Sale(int id, double quantity);
        Task<string> GenerateBarcode(int productId);
        Task<bool> MarkingForDeletion(int id);
        Task<bool> SearchByBarcode(string barcode);
        Task<IEnumerable<Product>> GetAllUnmarkedAsync();
        Task<IEnumerable<Product>> Report();
        Task<IEnumerable<ProductWareHouseView>> GetProducts();
        Task<IEnumerable<ProductView>> GetProductViewsAsync();
        Task<ProductView> GetProductViewByIdAsync(int id);

        event Action<ProductView> OnProductCreated;
        event Action<ProductView> OnProductUpdated;
        event Action<int, double> OnProductSaleOrRefund;
    }
}
