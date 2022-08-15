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
    public class RegistrationService : IRegistrationService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Registration> _nonQueryDataService;

        public RegistrationService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Registration>(_contextFactory);
        }

        public event Action PropertiesChanged;
        public event Action<Registration> OnEdited;
        public event Action<Registration> OnCreated;

        public async Task<Registration> CreateAsync(Registration entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                OnCreated?.Invoke(result);
                return result;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
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

        public IEnumerable<Registration> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Registrations
                    .Include(o => o.RegistrationProducts)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Registration>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Registrations
                    .Include(o => o.RegistrationProducts)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Registration> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Registrations
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Registration> UpdateAsync(int id, Registration entity)
        {
            return await _nonQueryDataService.Update(id, entity);
        }
    }
}
