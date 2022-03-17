using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IShiftService : IDataService<Shift>
    {
        /// <summary>
        /// Открыть новую смену для текущего кассира
        /// </summary>
        /// <param name="userId">Код кассира</param>
        /// <returns>Возврашает открытую смену</returns>
        Task<Shift> OpeningShiftAsync(int userId);

        /// <summary>
        /// Закрыть смену
        /// </summary>
        /// <param name="userId">Код кассира</param>
        Task<bool> ClosingShiftAsync(int userId);

        /// <summary>
        /// Получить открытую смену по коду кассира
        /// </summary>
        /// <param name="userId">Код кассира</param>
        /// <returns>Возврашает смену</returns>
        Task<Shift> GetOpenShiftAsync();

        /// <summary>
        /// Получить открытую смену
        /// </summary>
        /// <returns></returns>
        Task<Shift> GetOpenShift();

        Task<IEnumerable<Shift>> GetClosingShifts(DateTime startDate, DateTime endDate);
    }
}
