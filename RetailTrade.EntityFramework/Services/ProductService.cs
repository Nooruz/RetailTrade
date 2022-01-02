using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class ProductService : IProductService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Product> _nonQueryDataService;

        public event Action PropertiesChanged;
        public event Action<Product> OnProductCreated;
        public event Action<double> OnProductRefunded;
        public event Action<Product> OnProductEdited;

        public ProductService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Product>(_contextFactory);
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                OnProductCreated?.Invoke(await GetAsync(result.Id));
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<Product> UpdateAsync(int id, Product entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnProductEdited?.Invoke(result);
            return result;
        }

        public async Task<Product> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products                    
                    .Include(p => p.Supplier)
                    .Include(p => p.ProductSubcategory)
                    .ThenInclude(p => p.ProductCategory)
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Product> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Products
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .Include(p => p.Unit)
                    .Include(p => p.ProductSubcategory)
                    .ThenInclude(p => p.ProductCategory)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetByProductSubcategoryIdAsync(int? productSubcategoryId)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .Where(p => p.ProductSubcategoryId == productSubcategoryId)
                    .Include(p => p.Unit)
                    .Include(p => p.ProductSubcategory)
                    .ThenInclude(p => p.ProductCategory)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetByProductCategoryIdAsync(int? productCategoryId)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .Where(p => p.ProductSubcategory.ProductCategoryId == productCategoryId)
                    .Include(p => p.Unit)
                    .Include(p => p.ProductSubcategory)
                    .ThenInclude(p => p.ProductCategory)                 
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Product> GetForRefund(int supplierId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Products
                    .Where(p => p.Quantity > 0 && p.SupplierId == supplierId)
                    .Select(p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity })
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Product>> PredicateSelect(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                var result = await context.Products
                    .Where(predicate)
                    .Select(select)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Product> Predicate(Expression<Func<Product, bool>> predicate, Expression<Func<Product, Product>> select)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .Where(predicate)
                    .Select(select)
                    .FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<double> GetQuantity(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Product product = await context.Products
                                               .Where(p => p.Id == id)
                                               .Select(p => new Product { Quantity = p.Quantity })
                                               .FirstOrDefaultAsync();
                return product.Quantity;
            }
            catch (Exception e)
            {
                //ignore
            }
            return 0;
        }

        public async Task<double> Refund(int id, double quantity)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Product editProduct = await context.Products
                                               .Where(p => p.Id == id)
                                               .Select(p => new Product { Quantity = p.Quantity })
                                               .FirstOrDefaultAsync();
                editProduct.Quantity += quantity;

                await UpdateAsync(editProduct.Id, editProduct);

                return quantity;
            }
            catch (Exception e)
            {
                //ignore
            }
            return 0;
        }

        public async Task<string> GenerateBarcode(Product product)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();

                return "";
            }
            catch (Exception e)
            {
                //ignore
            }
            return "";
        }
    }
}
