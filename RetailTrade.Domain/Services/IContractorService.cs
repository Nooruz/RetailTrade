using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IContractorService : IDataService<Contractor>
    {
        event Action<Contractor> OnEdited;
        event Action<Contractor> OnCreated;
    }
}
