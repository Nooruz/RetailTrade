using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IReceiptService : IDataService<Receipt>
    {
        Task<IEnumerable<Receipt>> GetReceiptsAsync();
    }
}
