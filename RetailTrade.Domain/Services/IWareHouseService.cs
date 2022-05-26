using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IWareHouseService : IDataService<WareHouse>
    {
        event Action<WareHouse> OnWareHouseCreated;
        event Action<WareHouse> OnWareHouseEdited;
        Task MarkingForDeletion(WareHouse wareHouse);
    }
}
