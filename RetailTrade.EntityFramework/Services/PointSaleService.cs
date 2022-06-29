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
    public class PointSaleService : IPointSaleService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<PointSale> _nonQueryDataService;

        public PointSaleService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<PointSale>(_contextFactory);
        }

        public event Action<PointSale> OnCreated;
        public event Action<PointSale> OnEdited;
        public event Action<int> OnDeleted;

        public async Task<PointSale> CreateAsync(PointSale entity)
        {
            await using var context = _contextFactory.CreateDbContext();
            List<User> users = new();
            if (entity.Users != null && entity.Users.Any())
            {
                users = entity.Users;
                entity.Users = null;
            }
            PointSale result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                foreach (var item in users)
                {
                    _ = await context.UserPointSales.AddAsync(new UserPointSale
                    {
                        UserId = item.Id,
                        PointSaleId = result.Id
                    });
                }
                _ = await context.SaveChangesAsync();
                OnCreated?.Invoke(result);
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = await _nonQueryDataService.Delete(id);
            if (result)
            {
                OnDeleted?.Invoke(id);
            }
            return result;
        }

        public IEnumerable<PointSale> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.PointSales
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<PointSale>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.PointSales
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<PointSale> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.PointSales
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<PointSale> UpdateAsync(int id, PointSale entity)
        {
            PointSale result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
            {
                OnEdited?.Invoke(result);
            }
            return result;
        }
    }
}
