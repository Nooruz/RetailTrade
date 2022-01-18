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

        /// <summary>
        /// Сумма продажи за сегодяшний день
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Receipt>> GetSaleAmoundToday();

        /// <summary>
        /// Сумма продажи за вчерашний день
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Receipt>> GetSaleAmoundYesterday();

        /// <summary>
        /// Сумма продажи за предыдущую неделю
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Receipt>> GetSaleAmoundLastWeek();

        /// <summary>
        /// Сумма продажи за текущий месяц
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Receipt>> GetSaleAmoundCurrentMonth();

        /// <summary>
        /// Сумма продажи за прошлый месяц
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetSaleAmoundLastMonth();

        /// <summary>
        /// Сумма продажи с начала года
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetSaleAmoundBeginningYear();

        Task SetKKMCheckNumber(int id, string checkNumber);
    }
}