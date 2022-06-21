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
    public class ProductPriceService : IProductPriceService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ProductPrice> _nonQueryDataService;

        public ProductPriceService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<ProductPrice>(_contextFactory);
        }

        public event Action<ProductPrice> OnCreated;
        public event Action<ProductPrice> OnEdited;

        public async Task<ProductPrice> CreateAsync(ProductPrice entity)
        {
            await using var context = _contextFactory.CreateDbContext();
            ProductPrice result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                OnCreated?.Invoke(result);
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _nonQueryDataService.Delete(id);
        }

        public IEnumerable<ProductPrice> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ProductPrices.ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductPrice>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductPrices.ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<ProductPrice> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductPrices.FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<ProductPrice> UpdateAsync(int id, ProductPrice entity)
        {
            ProductPrice result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
            {
                OnEdited?.Invoke(result);
            }
            return result;
        }
    }
}
