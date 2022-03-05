using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using System;
using System.Threading.Tasks;

namespace RetailTradeClient.State.Shifts
{
    public enum CheckingResult
    {
        /// <summary>
        /// Смена открыта
        /// </summary>
        Open,


        /// <summary>
        /// Смена создана
        /// </summary>
        Created,

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
        /// Ошибка при открытии смены
        /// </summary>
        ErrorOpening,

        /// <summary>
        /// Ошибка при закрытии смены
        /// </summary>
        ErrorClosing,

        /// <summary>
        /// Неизвестная ошибка при закрытии
        /// </summary>
        UnknownErrorWhenClosing,

        /// <summary>
        /// 
        /// </summary>
        Nothing,

        /// <summary>
        /// Нет открытого смена
        /// </summary>
        NoOpenShift
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
        Task OpeningShift(IMessageBoxService MessageBoxService, int userId);

        /// <summary>
        /// Закрытие смены
        /// </summary>
        /// <param name="userId">Код кассира</param>
        Task ClosingShift(IMessageBoxService MessageBoxService, int userId);

        /// <summary>
        /// Проверка смены
        /// </summary>
        /// <returns>Если открыто true, иначи false</returns>
        Task CheckingShift(IMessageBoxService MessageBoxService, int userId);

        /// <summary>
        /// Обработчик
        /// </summary>
        event Action<CheckingResult> CurrentShiftChanged;
    }
}
