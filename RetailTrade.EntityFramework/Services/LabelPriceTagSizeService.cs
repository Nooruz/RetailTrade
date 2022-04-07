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
    public class LabelPriceTagSizeService : ILabelPriceTagSizeService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<LabelPriceTagSize> _nonQueryDataService;

        public LabelPriceTagSizeService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<LabelPriceTagSize>(_contextFactory);
        }

        public event Action PropertiesChanged;

        public async Task<LabelPriceTagSize> CreateAsync(LabelPriceTagSize entity)
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

        public IEnumerable<LabelPriceTagSize> GetAll()
        {
            try
            {
                using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return context.LabelPriceTagSizes.ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<LabelPriceTagSize>> GetAllAsync()
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return await context.LabelPriceTagSizes.ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<LabelPriceTagSize>> GetAllAsyncByTypeLabelPriceTagId(int typeLabelPriceTagId)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return await context.LabelPriceTagSizes.Where(l => l.TypeLabelPriceTagId == typeLabelPriceTagId).ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<LabelPriceTagSize> GetAsync(int id)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return await context.LabelPriceTagSizes.FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<LabelPriceTagSize> UpdateAsync(int id, LabelPriceTagSize entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
