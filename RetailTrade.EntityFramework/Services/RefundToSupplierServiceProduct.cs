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
    public class RefundToSupplierServiceProduct : IRefundToSupplierServiceProduct
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<RefundToSupplierProduct> _nonQueryDataService;
        private readonly IProductService _productService;

        public event Action PropertiesChanged;

        public RefundToSupplierServiceProduct(RetailTradeDbContextFactory contextFactory,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<RefundToSupplierProduct>(_contextFactory);
        }

        public async Task<RefundToSupplierProduct> CreateAsync(RefundToSupplierProduct entity)
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

        public async Task<RefundToSupplierProduct> UpdateAsync(int id, RefundToSupplierProduct entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<RefundToSupplierProduct> GetAsync(int id)
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

        public IEnumerable<RefundToSupplierProduct> GetAll()
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

        public async Task<IEnumerable<RefundToSupplierProduct>> GetAllAsync()
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

        public async Task<bool> AddRangeAsync(List<RefundToSupplierProduct> productRefunds)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                foreach (RefundToSupplierProduct item in productRefunds)
                {
                    await CreateAsync(new RefundToSupplierProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
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
