using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IArrivalProductService : IDataService<ArrivalProduct>
    {
        event Action<ArrivalProduct> OnEdited;
        event Action<ArrivalProduct> OnCreated;
        Task<bool> AddRangeAsync(List<ArrivalProduct> arrivalProducts);
        Task<bool> EditAsync(ArrivalProduct newArrivalProduct);
        Task<ArrivalProduct> GetByInclude(int id);
        Task<bool> CreateRangeAsync(int wareHouseId, int arrivalId, IEnumerable<ArrivalProduct> arrivalProducts);
    }
}
