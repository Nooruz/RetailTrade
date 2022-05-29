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
    public class UnitService : IUnitService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Unit> _nonQueryDataService;

        public UnitService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Unit>(_contextFactory);
        }

        public event Action<Unit> OnEdited;
        public event Action<Unit> OnCreated;

        public async Task<Unit> CreateAsync(Unit entity)
        {
            var result = await _nonQueryDataService.Create(entity);
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

        public IEnumerable<Unit> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Units.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Unit>> GetAllAsync()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return await context.Units.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Unit> GetAsync(int id)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return await context.Units.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Unit> UpdateAsync(int id, Unit entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnEdited?.Invoke(result);
            return result;
        }
    }
}
