using RetailTrade.POS.ViewModels;
using System;

namespace RetailTrade.POS.States.Navigators
{
    public enum MenuViewType
    {
        /// <summary>
        /// Продажи
        /// </summary>
        Sales,

        /// <summary>
        /// Возврат
        /// </summary>
        Refund,

        /// <summary>
        /// Смена
        /// </summary>
        Shift,

        /// <summary>
        /// История
        /// </summary>
        History,

        /// <summary>
        /// Отложенные чеки
        /// </summary>
        DeferredReceipt,

        /// <summary>
        /// Работа кассы
        /// </summary>
        WorkSale
    }

    public interface IMenuNavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
