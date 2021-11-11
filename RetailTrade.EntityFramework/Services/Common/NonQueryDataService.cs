using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services.Common
{
    public class NonQueryDataService<T> where T : DomainObject
    {
        private readonly RetailTradeDbContextFactory _contextFactory;

        public NonQueryDataService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                var createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return createdResult.Entity;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<T> Update(int id, T entity)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                entity.Id = id;
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                var entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                //ignore
            }
            return false;
        }
    }
}
