using RetailTrade.Domain.Models;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IOrganizationService : IDataService<Organization>
    {
        Task<Organization> GetCurrentOrganization();
    }
}
