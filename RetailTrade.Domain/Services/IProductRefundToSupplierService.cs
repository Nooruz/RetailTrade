using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductRefundToSupplierService : IDataService<ProductRefundToSupplier>
    {
        Task<bool> AddRangeAsync(List<ProductRefundToSupplier> productRefunds);
    }
}
