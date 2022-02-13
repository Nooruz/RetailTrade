using RetailTrade.Domain.Models;
using System;

namespace RetailTrade.Domain.Services
{
    public interface IRevaluationService : IDataService<Revaluation>
    {
        event Action<Revaluation> OnRevaluationCreated;
    }
}
