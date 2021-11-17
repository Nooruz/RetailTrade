using RetailTrade.Domain.Models;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IArrivalService : IDataService<Arrival>
    {
        Task<bool> Clone(int id);
    }
}
