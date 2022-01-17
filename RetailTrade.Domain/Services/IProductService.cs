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
        Task<IEnumerable<Product>> GetByProductSubcategoryIdAsync(int? productSubcategoryId);
        Task<IEnumerable<Product>> GetByProductCategoryIdAsync(int? productCategoryId);

        /// <summary>
        /// Получить товары для возврата поставшику
        /// </summary>
        /// <param name="supplierId">Код поставшика</param>
        /// <returns></returns>
        IEnumerable<Product> GetForRefund(int supplierId);
        Task<IEnumerable<Product>> PredicateSelect(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
        Task<Product> Predicate(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
        Task<double> GetQuantity(int id);
        Task<double> Refund(int id, double quantity);
        Task<string> GenerateBarcode(int productId);
        Task<bool> MarkingForDeletion(Product product);

        event Action<Product> OnProductCreated;
        event Action<Product> OnProductEdited;
        event Action<double> OnProductRefunded;
    }
}
