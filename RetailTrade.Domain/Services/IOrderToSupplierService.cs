using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IOrderToSupplierService : IDataService<OrderToSupplier>
    {
        event Action<OrderToSupplier> OnOrderToSupplierCreated;
    }
}
