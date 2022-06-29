using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services.Common
{
    public class WithoutKeyNonQueryDataService<T> where T : WithoutKeyDomainObject
    {
        private readonly RetailTradeDbContextFactory _contextFactory;

        public WithoutKeyNonQueryDataService(RetailTradeDbContextFactory contextFactory)
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
            catch (Exception)
            {
                //ignore
            }
            return null;
        }
    }
}
