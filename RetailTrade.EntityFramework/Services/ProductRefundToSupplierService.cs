using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class ProductRefundToSupplierService : IProductRefundToSupplierService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ProductRefundToSupplier> _nonQueryDataService;
        private readonly IProductService _productService;

        public event Action PropertiesChanged;

        public ProductRefundToSupplierService(RetailTradeDbContextFactory contextFactory,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<ProductRefundToSupplier>(_contextFactory);
        }

        public async Task<ProductRefundToSupplier> CreateAsync(ProductRefundToSupplier entity)
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

        public async Task<ProductRefundToSupplier> UpdateAsync(int id, ProductRefundToSupplier entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<ProductRefundToSupplier> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductRefunds
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<ProductRefundToSupplier> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ProductRefunds
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductRefundToSupplier>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductRefunds
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> AddRangeAsync(List<ProductRefundToSupplier> productRefunds)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                foreach (ProductRefundToSupplier item in productRefunds)
                {
                    await CreateAsync(new ProductRefundToSupplier
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        RefundDate = DateTime.Now
                    });
                    Product product = await _productService.GetByIdAsync(item.ProductId);
                    product.Quantity -= item.Quantity;
                    await _productService.UpdateAsync(product.Id, product);
                }
                return true;
            }
            catch (Exception e)
            {
                //ignore
                return false;
            }
        }
    }
}
