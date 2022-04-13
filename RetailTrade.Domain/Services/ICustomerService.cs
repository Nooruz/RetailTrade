using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface ICustomerService : IDataService<Customer>
    {
        event Action<Customer> OnEdited;
        event Action<Customer> OnCreated;
    }
}
