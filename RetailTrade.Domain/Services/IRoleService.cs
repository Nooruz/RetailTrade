using RetailTrade.Domain.Models;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IRoleService : IDataService<Role>
    {
        Task<Role> GetFirstOrDefaultAsync();
    }
}
