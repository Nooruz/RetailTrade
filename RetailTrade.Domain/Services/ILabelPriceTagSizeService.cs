using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface ILabelPriceTagSizeService : IDataService<LabelPriceTagSize>
    {
        Task<IEnumerable<LabelPriceTagSize>> GetAllAsyncByTypeLabelPriceTagId(int typeLabelPriceTagId);
    }
}
