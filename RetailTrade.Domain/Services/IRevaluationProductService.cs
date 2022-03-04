using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IRevaluationProductService : IDataService<RevaluationProduct>
    {
        Task<IEnumerable<RevaluationProduct>> GetAllByRevaluationId(int revaluationId);
    }
}
