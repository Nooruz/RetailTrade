using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IPointSaleService : IDataService<PointSale>
    {
        event Action<PointSale> OnCreated;
        event Action<PointSale> OnEdited;
        event Action<int> OnDeleted;

        Task<PointSale> GetPointSaleUserAsync(int id);
    }
}
