using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IWareHouseService : IDataService<WareHouse>
    {
        event Action<WareHouse> OnWareHouseCreated;
    }
}
