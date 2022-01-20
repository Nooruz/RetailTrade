using RetailTradeServer.ViewModels.Base;
using System;

namespace RetailTradeServer.State.Navigators
{
    public enum MenuViewType
    {
        /// <summary>
        /// Товары
        /// </summary>
        Products,

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
        /// Панель мониторинга продаж и доходов
        /// </summary>
        SaleDashboard,

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
        RefundToSupplier,

        /// <summary>
        /// Поставщики
        /// </summary>
        Supplier,

        /// <summary>
        /// Сотрудники
        /// </summary>
        Employee,

        /// <summary>
        /// Подключение и настройка оборудования
        /// </summary>
        ConnectingAndConfiguringEquipment,

        /// <summary>
        /// Кассовые смены
        /// </summary>
        CashierView,

        /// <summary>
        /// Организация
        /// </summary>
        OrganizationView
    }
    public interface IMenuNavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        event Action StateChanged;
    }
}
