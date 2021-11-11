using RetailTrade.Domain.Models;
using System.Collections.Generic;

namespace RetailTrade.Domain.Services
{
    public interface ISupplierService : IDataService<Supplier>
    {
        IEnumerable<Supplier> GetOnlyNames();        
    }
}
