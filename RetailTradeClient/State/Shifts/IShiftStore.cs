using DevExpress.Mvvm;
using RetailTrade.Domain.Models;
using System;
using System.Collections.ObjectModel;
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
        NoOpenShift,

        /// <summary>
        /// Смена открыта другим пользователем
        /// </summary>
        ShiftOpenedByAnotherUser
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

        ObservableCollection<Receipt> GetReceipts();

        /// <summary>
        /// Открытие смены
        /// </summary>
        /// <returns>Если открыто true, иначи false</returns>
        Task OpeningShift(IMessageBoxService MessageBoxService);

        /// <summary>
        /// Закрытие смены
        /// </summary>
        Task ClosingShift(IMessageBoxService MessageBoxService);

        /// <summary>
        /// Проверка смены
        /// </summary>
        /// <returns>Если открыто true, иначи false</returns>
        Task CheckingShift(IMessageBoxService MessageBoxService);

        /// <summary>
        /// Обработчик
        /// </summary>
        event Action<CheckingResult> CurrentShiftChanged;
    }
}
