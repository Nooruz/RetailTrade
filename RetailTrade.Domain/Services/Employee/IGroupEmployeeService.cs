using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IGroupEmployeeService : IDataService<GroupEmployee>
    {
        Task<IEnumerable<GroupEmployeeDTO>> GetDTOAllAsync();
    }
}
