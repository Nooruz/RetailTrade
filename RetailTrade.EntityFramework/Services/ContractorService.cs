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
    public class ContractorService : IContractorService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Contractor> _nonQueryDataService;

        public ContractorService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Contractor>(_contextFactory);
        }

        public event Action PropertiesChanged;
        public event Action<Contractor> OnEdited;
        public event Action<Contractor> OnCreated;

        public async Task<Contractor> CreateAsync(Contractor entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await _nonQueryDataService.Delete(id);
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public IEnumerable<Contractor> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Contractors
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Contractor>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Contractors
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Contractor> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Contractors
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Contractor> UpdateAsync(int id, Contractor entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnEdited?.Invoke(result);
            return result;
        }
    }
}
