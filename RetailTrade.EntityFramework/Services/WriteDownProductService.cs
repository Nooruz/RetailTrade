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
    public class WriteDownProductService : IWriteDownProductService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<WriteDownProduct> _nonQueryDataService;
        private readonly IProductService _productService;

        public event Action PropertiesChanged;

        public WriteDownProductService(RetailTradeDbContextFactory contextFactory, 
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<WriteDownProduct>(_contextFactory);
        }

        public async Task<WriteDownProduct> CreateAsync(WriteDownProduct entity)
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

        public async Task<WriteDownProduct> UpdateAsync(int id, WriteDownProduct entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<WriteDownProduct> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.WriteDownProducts
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<WriteDownProduct> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.WriteDownProducts
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<WriteDownProduct>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.WriteDownProducts
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> AddRangeAsync(List<WriteDownProduct> writeDownProducts)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                foreach (WriteDownProduct item in writeDownProducts)
                {
                    await CreateAsync(new WriteDownProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                    Product product = await _productService.GetByIdAsync(item.ProductId);
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
