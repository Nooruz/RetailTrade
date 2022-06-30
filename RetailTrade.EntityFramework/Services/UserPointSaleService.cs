using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class UserPointSaleService : IUserPointSaleService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly WithoutKeyNonQueryDataService<UserPointSale> _nonQueryDataService;

        public UserPointSaleService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new WithoutKeyNonQueryDataService<UserPointSale>(_contextFactory);
        }

        public async Task<bool> AddRangeAsync(List<UserPointSale> values)
        {
            try
            {
                var context = _contextFactory.CreateDbContext();
                await context.UserPointSales.AddRangeAsync(values);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }
    }
}
