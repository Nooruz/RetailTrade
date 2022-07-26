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
    public class RefundToSupplierService : IRefundToSupplierService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<RefundToSupplier> _nonQueryDataService;
        private readonly IProductService _productService;

        public RefundToSupplierService(RetailTradeDbContextFactory contextFactory,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<RefundToSupplier>(_contextFactory);
        }

        public event Action PropertiesChanged;

        public async Task<bool> Clone(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                RefundToSupplier RefundToSupplier = await context.RefundsToSuppliers
                    .Include(a => a.RefundToSupplierProducts)
                    .FirstOrDefaultAsync(a => a.Id == id);

                List<RefundToSupplierProduct> refundToSupplierProducts = new();

                foreach (var item in RefundToSupplier.RefundToSupplierProducts)
                {
                    refundToSupplierProducts.Add(new RefundToSupplierProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }

                RefundToSupplier.Id = 0;
                RefundToSupplier.RefundToSupplierDate = DateTime.Now;
                RefundToSupplier.RefundToSupplierProducts = refundToSupplierProducts;

                await CreateAsync(RefundToSupplier);
            }
            catch (Exception e)
            {
                //ignore
            }

            return true;
        }

        public async Task<RefundToSupplier> CreateAsync(RefundToSupplier entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                if (entity.RefundToSupplierProducts.Count > 0)
                {
                    foreach (var item in entity.RefundToSupplierProducts)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
                        _ = await _productService.UpdateAsync(product.Id, product);
                    }
                }
                PropertiesChanged?.Invoke();
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                RefundToSupplier RefundToSupplier = await context.RefundsToSuppliers
                    .Include(a => a.RefundToSupplierProducts)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (RefundToSupplier.RefundToSupplierProducts.Count > 0)
                {
                    foreach (var item in RefundToSupplier.RefundToSupplierProducts)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
                        _ = await _productService.UpdateAsync(product.Id, product);
                    }
                }

                var result = await _nonQueryDataService.Delete(id);

                if (result)
                {
                    PropertiesChanged?.Invoke();
                }

                return result;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public IEnumerable<RefundToSupplier> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.RefundsToSuppliers
                    .Include(o => o.RefundToSupplierProducts)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<RefundToSupplier>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.RefundsToSuppliers
                    .Include(o => o.RefundToSupplierProducts)
                    .ThenInclude(o => o.Product)
                    .Include(o => o.Supplier)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<RefundToSupplier> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.RefundsToSuppliers
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<RefundToSupplier> UpdateAsync(int id, RefundToSupplier entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
