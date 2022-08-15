using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IRegistrationService : IDataService<Registration>
    {
        event Action PropertiesChanged;
        event Action<Registration> OnEdited;
        event Action<Registration> OnCreated;
    }
}
