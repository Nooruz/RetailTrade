using RetailTrade.Domain.Models;

namespace RetailTrade.Domain.Services
{
    public interface IOrganizationService : IDataService<Organization>
    {
        Organization Get();
    }
}
