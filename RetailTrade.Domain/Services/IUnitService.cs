using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IUnitService : IDataService<Unit>
    {
        public event Action<Unit> OnEdited;
        public event Action<Unit> OnCreated;
    }
}
