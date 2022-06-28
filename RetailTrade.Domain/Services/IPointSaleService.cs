using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IPointSaleService : IDataService<PointSale>
    {
        event Action<PointSale> OnCreated;
        event Action<PointSale> OnEdited;
        event Action<int> OnDeleted;
    }
}
