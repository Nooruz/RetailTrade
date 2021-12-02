using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IReceiptService : IDataService<Receipt>
    {
        /// <summary>
        /// Получить квитанции из текущего смена
        /// </summary>
        /// <returns>Список квитанции</returns>
        Task<IEnumerable<Receipt>> GetReceiptsFromCurrentShift(int shiftId);

        Task<bool> Refund(Receipt receipt);

        Task<IEnumerable<Receipt>> Predicate(Expression<Func<Receipt, bool>> predicate, Expression<Func<Receipt, Receipt>> select);
    }
}