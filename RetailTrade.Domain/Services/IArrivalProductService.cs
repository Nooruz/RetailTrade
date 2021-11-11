using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IArrivalProductService : IDataService<ArrivalProduct>
    {
        Task<bool> AddRangeAsync(List<ArrivalProduct> arrivalProducts);
    }
}
