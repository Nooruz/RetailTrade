using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class ProductService : IProductService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Product> _nonQueryDataService;

        public event Action PropertiesChanged;

        public ProductService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Product>(_contextFactory);
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                PropertiesChanged?.Invoke();
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
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<Product> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
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

        public async Task<Product> GetFirstOrDefaultAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Product> GetByInclude()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Products
                    .Include(p => p.Unit)
                    .Include(p => p.ProductSubcategory)
                    .ThenInclude(p => p.ProductCategory)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Product> GetByProductSubcategoryId(int productSubcategoryId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Products
                    .Where(p => p.ProductSubcategoryId == productSubcategoryId)
                    .Include(p => p.Unit)
                    .Include(p => p.ProductSubcategory)
                    .ThenInclude(p => p.ProductCategory)
                    .ToList();
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

        public IEnumerable<Product> GetByProductCategoryId(int productCategoryId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Products
                    .Where(p => p.ProductSubcategory.ProductCategoryId == productCategoryId)
                    .Include(p => p.Unit)
                    .Include(p => p.ProductSubcategory)
                    .ThenInclude(p => p.ProductCategory)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Product> GetBySupplierId(int supplierId)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Products
                    .Where(p => p.SupplierId == supplierId)
                    .Select(p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity })
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }
        
        public async Task<bool> ArrivalProducts(List<ArrivalProduct> arrivalProducts)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                foreach (ArrivalProduct arrivalProduct in arrivalProducts)
                {
                    Product product = await GetByIdAsync(arrivalProduct.Id);
                    product.Quantity += arrivalProduct.Quantity;
                    await UpdateAsync(product.Id, product);
                }
                return true;
            }
            catch
            {
                //ignore
                return false;
            }
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

        public async Task<Product> GetByBarcodeAsync(string barcode)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .FirstOrDefaultAsync(p => p.Barcode == barcode);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public Product GetByBarcode(string barcode)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Products.FirstOrDefault(p => p.Barcode == barcode);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Product> GetByWithoutBarcode()
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

        public async Task<IEnumerable<Product>> Queryable()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .Select(p => new Product { Id = p.Id, Name = p.Name, Barcode = p.Barcode, Quantity = p.Quantity })
                    .ToListAsync();
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
    }
}
