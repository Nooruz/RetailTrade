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
        Task<IEnumerable<User>> GetAdminAsync();
        Task<IEnumerable<User>> GetCashiersAsync();

        /// <summary>
        /// Поментка на удаление
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        Task<bool> MarkingForDeletion(User user);

        event Action<User> OnUserCreated;
        event Action<User> OnUserUpdated;
    }
}
