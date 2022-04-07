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
    public class GenericService<T> : IDataService<T> where T : DomainObject
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<T> _nonQueryDataService;

        public event Action PropertiesChanged;

        public GenericService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<T>(_contextFactory);
        }

        public async Task<T> CreateAsync(T entity)
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

        public async Task<T> GetAsync(int id)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return await context.Set<T>().ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
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
            try
            {
                using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return context.Set<T>().ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }
    }
}
