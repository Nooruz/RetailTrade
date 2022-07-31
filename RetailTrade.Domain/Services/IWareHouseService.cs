using RetailTrade.Domain.Models;
using RetailTrade.Domain.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IWareHouseService : IDataService<WareHouse>
    {
        event Action<WareHouse> OnWareHouseCreated;
        event Action<WareHouse> OnWareHouseEdited;
        Task MarkingForDeletion(WareHouse wareHouse);
        Task<IEnumerable<WareHouseView>> GetByProductId(int productId);
    }
}
