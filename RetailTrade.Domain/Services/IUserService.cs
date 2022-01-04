using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IUserService : IDataService<User>
    {
        Task<User> GetByUsername(string username);
        Task<bool> AnyAsync();
        IEnumerable<User> GetCashiers();

        event Action<User> OnUserCreated;
    }
}
