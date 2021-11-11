using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IWriteDownProductService : IDataService<WriteDownProduct>
    {
        Task<bool> AddRangeAsync(List<WriteDownProduct> writeDownProducts);
    }
}
