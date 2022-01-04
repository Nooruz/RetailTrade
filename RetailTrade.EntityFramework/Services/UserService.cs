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
    public class UserService : IUserService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<User> _nonQueryDataService;

        public event Action PropertiesChanged;
        public event Action<User> OnUserCreated;

        public UserService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<User>(_contextFactory);
        }

        public async Task<User> CreateAsync(User entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                OnUserCreated?.Invoke(await GetAsync(result.Id));
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<User> UpdateAsync(int id, User entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<User> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Users
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Users
                            .Include(u => u.Role)
                            .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Users
                    .Include(u => u.Role)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<User> GetByUsername(string username)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(a => a.Username == username);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> AnyAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Users.AnyAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return false;
        }

        public IEnumerable<User> GetCashiers()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Users
                    .Where(u => u.Role.Name == "Кассир")
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }
    }
}
