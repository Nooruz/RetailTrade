using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IRefundToSupplierServiceProduct : IDataService<RefundToSupplierProduct>
    {
        Task<bool> AddRangeAsync(List<RefundToSupplierProduct> productRefunds);
    }
}
