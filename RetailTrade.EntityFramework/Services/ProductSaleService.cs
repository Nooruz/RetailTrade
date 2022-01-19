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
    public class ProductSaleService : IProductSaleService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ProductSale> _nonQueryDataService;
        private readonly IProductService _productService;

        public ProductSaleService(RetailTradeDbContextFactory contextFactory,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<ProductSale>(_contextFactory);
        }

        public event Action PropertiesChanged;

        public async Task<ProductSale> CreateAsync(ProductSale entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                try
                {
                    Product product = await _productService.GetByIdAsync(entity.ProductId);
                    product.Quantity -= entity.Quantity;
                    _ = await _productService.UpdateAsync(product.Id, product);
                    PropertiesChanged?.Invoke();
                }
                catch (Exception e)
                {
                    //ignore
                }                
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {                
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

        public IEnumerable<ProductSale> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ProductSales
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductSale>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductSales
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<ProductSale> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductSales
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductSale>> GetProductSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductSales
                    .Include(p => p.Receipt)
                    .Where(p => p.Receipt.DateOfPurchase.Date >= startDate && p.Receipt.DateOfPurchase.Date <= endDate)
                    .Select(p => new ProductSale { ArrivalPrice = p.ArrivalPrice, SalePrice = p.SalePrice, Quantity = p.Quantity })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductSale>> GetRatingTenProducts()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                var result = await context.ProductSales
                    .Include(p => p.Product)
                    .Include(p => p.Receipt)
                    .Where(p => p.Receipt.IsRefund == false)
                    .GroupBy(p => new { p.ProductId, p.Product.Name})
                    .Select(p => new ProductSale { ProductId = p.Key.ProductId, Product = new Product { Name = p.Key.Name }, Quantity = p.Sum(c => c.Quantity) })                    
                    .Take(10)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<ProductSale> UpdateAsync(int id, ProductSale entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
