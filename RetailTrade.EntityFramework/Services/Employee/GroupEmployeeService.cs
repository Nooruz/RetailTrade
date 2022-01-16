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
    public class GroupEmployeeService : IGroupEmployeeService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<GroupEmployee> _nonQueryDataService;

        public GroupEmployeeService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<GroupEmployee>(_contextFactory);
        }

        public event Action PropertiesChanged;

        public async Task<GroupEmployee> CreateAsync(GroupEmployee entity)
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

        public async Task<GroupEmployee> UpdateAsync(int id, GroupEmployee entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<GroupEmployee> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.GroupsEmployees
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<GroupEmployee>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.GroupsEmployees
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<GroupEmployee> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.GroupsEmployees
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }
    }
}
