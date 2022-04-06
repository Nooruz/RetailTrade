using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class LabelPriceTagService : ILabelPriceTagService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<LabelPriceTag> _nonQueryDataService;

        public event Action PropertiesChanged;

        public LabelPriceTagService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<LabelPriceTag>(_contextFactory);
        }

        public async Task<LabelPriceTag> CreateAsync(LabelPriceTag entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                oned?.Invoke();
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<T> GetAsync(int id)
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();
                return entities;
            }
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<T> GetAll()
        {
            using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T> entities = context.Set<T>().ToList();
                return entities;
            }
        }
    }
}
