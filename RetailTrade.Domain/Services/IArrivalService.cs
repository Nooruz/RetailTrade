using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IArrivalService : IDataService<Arrival>
    {
        event Action<Arrival> OnEdited;
        event Action<Arrival> OnCreated;
        Task<bool> Clone(int id);
        Task<Arrival> GetByIncludAsync(int id);
    }
}
