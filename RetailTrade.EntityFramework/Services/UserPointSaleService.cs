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

        public event Action<UserPointSale> OnCreated;
        public event Action<UserPointSale> OnEdited;
        public event Action<int> OnDeleted;

        public Task<UserPointSale> CreateAsync(UserPointSale entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserPointSale> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserPointSale>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserPointSale> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserPointSale> UpdateAsync(int id, UserPointSale entity)
        {
            throw new NotImplementedException();
        }
    }
}
