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
    public class RefundService : IRefundService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Refund> _nonQueryDataService;
        private readonly IProductService _productService;

        public RefundService(RetailTradeDbContextFactory contextFactory,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<Refund>(_contextFactory);
        }

        public event Action PropertiesChanged;

        public async Task<bool> Clone(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Refund Refund = await context.Refunds
                    .Include(a => a.ProductRefunds)
                    .FirstOrDefaultAsync(a => a.Id == id);

                List<ProductRefund> productRefunds = new();

                foreach (var item in Refund.ProductRefunds)
                {
                    productRefunds.Add(new ProductRefund
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Sum = item.Sum,
                        SalePrice = item.SalePrice
                    });
                }

                Refund.Id = 0;
                Refund.DateOfRefund = DateTime.Now;
                Refund.ProductRefunds = productRefunds;

                await CreateAsync(Refund);
            }
            catch (Exception e)
            {
                //ignore
            }

            return true;
        }

        public async Task<Refund> CreateAsync(Refund entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                if (entity.ProductRefunds.Count > 0)
                {
                    foreach (var item in entity.ProductRefunds)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
                        product.Quantity += item.Quantity;
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
                Refund refund = await context.Refunds
                    .Include(a => a.ProductRefunds)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (refund.ProductRefunds.Count > 0)
                {
                    foreach (var item in refund.ProductRefunds)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
                        product.Quantity -= item.Quantity;
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

        public IEnumerable<Refund> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Refunds
                    .Include(o => o.ProductRefunds)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Refund>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Refunds
                    .Include(o => o.ProductRefunds)
                    .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Unit)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Refund> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Refunds
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Refund> UpdateAsync(int id, Refund entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
