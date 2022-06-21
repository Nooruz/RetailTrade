using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductPriceService : IDataService<ProductPrice>
    {
        event Action<ProductPrice> OnCreated;
        event Action<ProductPrice> OnEdited;
    }
}
