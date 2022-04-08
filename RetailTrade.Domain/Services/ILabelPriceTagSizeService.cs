using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface ILabelPriceTagSizeService : IDataService<LabelPriceTagSize>
    {
        event Action<LabelPriceTagSize> OnCreated;
        Task<IEnumerable<LabelPriceTagSize>> GetAllAsyncByTypeLabelPriceTagId(int typeLabelPriceTagId);
    }
}
