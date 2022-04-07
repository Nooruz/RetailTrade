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
    public class LabelPriceTagService : ILabelPriceTagService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<LabelPriceTag> _nonQueryDataService;

        public event Action PropertiesChanged;
        public event Action<LabelPriceTag> OnCreated;
        public event Action<int> OnDeleted;
        public event Action<LabelPriceTag> OnEdited;

        public LabelPriceTagService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<LabelPriceTag>(_contextFactory);
        }

        public async Task<LabelPriceTag> CreateAsync(LabelPriceTag entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                OnCreated?.Invoke(result);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                OnDeleted?.Invoke(id);
            return result;
        }

        public async Task<LabelPriceTag> GetAsync(int id)
        {
            await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            return await context.LabelPriceTags.FirstOrDefaultAsync((e) => e.Id == id);
        }

        public async Task<IEnumerable<LabelPriceTag>> GetAllAsync()
        {
            await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            return await context.LabelPriceTags.ToListAsync();
        }

        public async Task<LabelPriceTag> UpdateAsync(int id, LabelPriceTag entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnEdited?.Invoke(result);
            return result;
        }

        public IEnumerable<LabelPriceTag> GetAll()
        {
            using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            return context.LabelPriceTags.ToList();
        }
    }
}
