using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IReceiptService : IDataService<Receipt>
    {
        Task<IEnumerable<Receipt>> GetReceiptsAsync();
        Task<IEnumerable<Receipt>> GetReceiptsByDateAsync(DateTime dateTime);
    }
}
