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
        Task<Product> GetByBarcodeAsync(string barcode);
        Product GetByBarcode(string barcode);
        IEnumerable<Product> GetByInclude();

        /// <summary>
        /// Получить товары без штрих кодов
        /// </summary>
        /// <returns></returns>
        IEnumerable<Product> GetByWithoutBarcode();

        /// <summary>
        /// Получить товары по категориям
        /// </summary>
        /// <param name="productSubcategoryId"></param>
        /// <returns></returns>
        IEnumerable<Product> GetByProductSubcategoryId(int productSubcategoryId);
        Task<IEnumerable<Product>> GetByProductSubcategoryIdAsync(int? productSubcategoryId);

        IEnumerable<Product> GetByProductCategoryId(int productCategoryId);
        Task<IEnumerable<Product>> GetByProductCategoryIdAsync(int? productCategoryId);
        IEnumerable<Product> GetBySupplierId(int supplierId);

        /// <summary>
        /// Приход товара
        /// </summary>
        /// <param name="arrivalProducts">Товары</param>
        Task<bool> ArrivalProducts(List<ArrivalProduct> arrivalProducts);

        Task<IEnumerable<Product>> Queryable();

        /// <summary>
        /// Получить товары для возврата поставшику
        /// </summary>
        /// <param name="supplierId">Код поставшика</param>
        /// <returns></returns>
        IEnumerable<Product> GetForRefund(int supplierId);
        Task<IEnumerable<Product>> PredicateSelect(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
        Task<Product> Predicate(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select);
    }
}
