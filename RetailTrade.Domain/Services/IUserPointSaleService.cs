using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IUserPointSaleService
    {
        Task<bool> AddRangeAsync(List<UserPointSale> values);
    }
}
