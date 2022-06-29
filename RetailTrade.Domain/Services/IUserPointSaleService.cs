using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IUserPointSaleService : IDataService<UserPointSale>
    {
        event Action<UserPointSale> OnCreated;
        event Action<UserPointSale> OnEdited;
        event Action<int> OnDeleted;
    }
}
