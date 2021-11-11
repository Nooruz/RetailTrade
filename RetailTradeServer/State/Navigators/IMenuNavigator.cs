using RetailTradeServer.ViewModels.Base;
using System;

namespace RetailTradeServer.State.Navigators
{
    public enum MenuViewType
    {
        /// <summary>
        /// Товары
        /// </summary>
        ProductCategory,

        /// <summary>
        /// Приход товара
        /// </summary>
        ArrivalProduct,

        /// <summary>
        /// Списание товара
        /// </summary>
        WriteDownProduct,

        /// <summary>
        /// Заказ товара
        /// </summary>
        OrderProduct,

        /// <summary>
        /// Ценники и этикетки
        /// </summary>
        ProductBarcode,

        /// <summary>
        /// Аналитическая панель
        /// </summary>
        AnalyticalPanel,

        /// <summary>
        /// Филиалы
        /// </summary>
        Branch,

        /// <summary>
        /// Пользователи
        /// </summary>
        User,

        /// <summary>
        /// Возврат поставшику
        /// </summary>
        RefundToSupplier
    }
    public interface IMenuNavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
