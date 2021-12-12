using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Services
{
    public interface ISupplierService : IDataService<Supplier>
    {
        IEnumerable<Supplier> GetOnlyNames();

        event Action<Supplier> OnSupplierCreated;
        event Action<Supplier> OnSupplierUpdated;
    }
}
