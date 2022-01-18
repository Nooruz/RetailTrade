using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Shifts
{
    public enum CheckingResult
    {
        /// <summary>
        /// Смена создана
        /// </summary>
        Open,

        /// <summary>
        /// Смена закрыта
        /// </summary>
        Close,

        /// <summary>
        /// Смена уже открыта
        /// </summary>
        IsAlreadyOpen,

        /// <summary>
        /// Смена просрочена
        /// </summary>
        Exceeded,

        /// <summary>
        /// Ошибка открытия смены ККМ
        /// </summary>
        ErrorOpeningShiftKKM,

        /// <summary>
        /// Неизвестная ошибка при закрытии
        /// </summary>
        UnknownErrorWhenClosing
    }

    /// <summary>
    /// Управление сменами
    /// </summary>
    public interface IShiftStore
    {
        /// <summary>
        /// Открыто ли смена
        /// </summary>
        bool IsShiftOpen { get; }

        /// <summary>
        /// Текущая открытая смена
        /// </summary>
        Shift CurrentShift { get; }

        /// <summary>
        /// Открытие смены
        /// </summary>
        /// <param name="userId">Код кассира</param>
        /// <returns>Если открыто true, иначи false</returns>
        Task<CheckingResult> OpeningShift(int userId);

        /// <summary>
        /// Закрытие смены
        /// </summary>
        /// <param name="userId">Код кассира</param>
        Task<CheckingResult> ClosingShift(int userId);

        /// <summary>
        /// Проверка смены
        /// </summary>
        /// <returns>Если открыто true, иначи false</returns>
        Task<CheckingResult> CheckingShift(int userId);

        /// <summary>
        /// Обработчик
        /// </summary>
        event Action CurrentShiftChanged;
    }
}
