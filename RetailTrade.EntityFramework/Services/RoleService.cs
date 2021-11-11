using Microsoft.EntityFrameworkCore;
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
    public class RoleService : IRoleService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Role> _nonQueryDataService;

        public event Action PropertiesChanged;

        public RoleService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Role>(_contextFactory);
        }

        public async Task<Role> CreateAsync(Role entity)
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

        public async Task<Role> UpdateAsync(int id, Role entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<Role> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Roles
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Role> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Roles
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Roles
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Role> GetFirstOrDefaultAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Roles.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }
    }
}
