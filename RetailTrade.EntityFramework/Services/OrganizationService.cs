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
    public class OrganizationService : IOrganizationService
    {
        #region Private Members

        private RetailTradeDbContextFactory _contextFactory;
        private NonQueryDataService<Organization> _nonQueryDataService;

        #endregion

        #region Constructor

        public OrganizationService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Organization>(_contextFactory);
        }

        #endregion

        public event Action PropertiesChanged;

        public async Task<Organization> CreateAsync(Organization entity)
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

        public Organization Get()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Organizations.FirstOrDefault();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Organization> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Organizations
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Organization>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Organizations
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Organization> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Organizations
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Organization> GetCurrentOrganization()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Organizations
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Organization> UpdateAsync(int id, Organization entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
