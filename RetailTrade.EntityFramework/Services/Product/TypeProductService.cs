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
    public class TypeProductService : ITypeProductService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<TypeProduct> _nonQueryDataService;

        public TypeProductService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<TypeProduct>(_contextFactory);
        }

        public event Action PropertiesChanged;
        public event Action<TypeProduct> OnTypeProductCreated;

        public async Task<TypeProduct> CreateAsync(TypeProduct entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            OnTypeProductCreated?.Invoke(result);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<TypeProduct> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.TypeProducts
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<TypeProduct>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.TypeProducts
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<TypeProduct> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.TypeProducts
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<TypeProduct>> GetGroupsAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.TypeProducts
                    .Where(g => g.IsGroup == true)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<TypeProduct>> GetTypesAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.TypeProducts
                    .Where(g => g.IsGroup == false)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<TypeProduct> UpdateAsync(int id, TypeProduct entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
