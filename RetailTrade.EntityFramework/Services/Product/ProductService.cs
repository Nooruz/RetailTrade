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
        public event Action<int, double> OnProductSaleOrRefund;

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
            return await _nonQueryDataService.Update(id, entity); ;
        }

        public async Task<Product> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
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
            catch (Exception)
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
                    .ToListAsync();
            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
                    //.Where(p => p.Quantity > 0 && p.SupplierId == supplierId)
                    .Select(p => new Product { Id = p.Id, Name = p.Name, Quantity = p.Quantity })
                    .ToList();
            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
                                               .Select(p => new Product { Id = p.Id, Quantity = p.Quantity })
                                               .FirstOrDefaultAsync();
                return product.Quantity;
            }
            catch (Exception)
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
                                               .FirstOrDefaultAsync();
                editProduct.Quantity += quantity;

                Product result = await UpdateAsync(id, editProduct);

                OnProductSaleOrRefund?.Invoke(result.Id, result.Quantity);

                return quantity;
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        public async Task<string> GenerateBarcode(int productId)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Product product = await GetAsync(productId);
                product.Barcode = $"2{new('0', 13 - product.Id.ToString().Length)}{product.Id}";
                Product updateProduct = await UpdateAsync(productId, product);
                OnProductEdited?.Invoke(updateProduct);
                return updateProduct.Barcode;
            }
            catch (Exception)
            {
                //ignore
            }
            return "";
        }

        public async Task<bool> MarkingForDeletion(Product product)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                product.DeleteMark = !product.DeleteMark;
                Product result = await UpdateAsync(product.Id, product);
                if (result != null)
                {
                    OnProductEdited?.Invoke(result);
                    return true;
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public async Task<double> Sale(int id, double quantity, bool isKeepRecrods)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Product editProduct = await context.Products
                                               .Where(p => p.Id == id)
                                               .FirstOrDefaultAsync();
                if (isKeepRecrods)
                {
                    editProduct.Quantity -= quantity;
                }

                Product result = await UpdateAsync(id, editProduct);

                OnProductSaleOrRefund?.Invoke(result.Id, result.Quantity);

                return quantity;
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        public async Task<bool> Refunds(IEnumerable<ProductRefund> productRefunds)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();

                foreach (ProductRefund item in productRefunds)
                {
                    Product editProduct = await context.Products
                                               .Where(p => p.Id == item.ProductId)
                                               .FirstOrDefaultAsync();
                    editProduct.Quantity += item.Quantity;

                    Product result = await UpdateAsync(editProduct.Id, editProduct);

                    OnProductSaleOrRefund?.Invoke(result.Id, result.Quantity);
                }
                return true;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public async Task<bool> SearchByBarcode(string barcode)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products.FirstOrDefaultAsync(p => p.Barcode == barcode) != null;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllUnmarkedAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Products
                    .Where(p => p.DeleteMark == false)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }
    }
}
