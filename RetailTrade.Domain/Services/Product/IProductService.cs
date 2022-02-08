using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductService : IDataService<Product>
    {
        Task<Product> GetByIdAsync(int id);
        IEnumerable<Product> GetForRefund(int supplierId);
        Task<IEnumerable<Product>> PredicateSelect(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
        Task<Product> Predicate(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
        Task<double> GetQuantity(int id);
        Task<double> Refund(int id, double quantity);
        Task<bool> Refunds(IEnumerable<ProductRefund> productRefunds);
        Task<double> Sale(int id, double quantity);
        Task<string> GenerateBarcode(int productId);
        Task<bool> MarkingForDeletion(Product product);
        Task<bool> SearchByBarcode(string barcode);

        event Action<Product> OnProductCreated;
        event Action<Product> OnProductEdited;
        event Action<double> OnProductRefunded;
        event Action<int, double> OnProductSaleOrRefund;
    }
}
