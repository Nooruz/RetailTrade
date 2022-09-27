using RetailTrade.Domain.Models;
using RetailTrade.Domain.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public enum DocumentTypeEnum
    {
        /// <summary>
        /// Приемка
        /// </summary>
        Supply = 1,

        /// <summary>
        /// Возврат поставщику
        /// </summary>
        PurchaseReturn = 2,

        /// <summary>
        /// Списание
        /// </summary>
        Loss = 3,

        /// <summary>
        /// Оприходование
        /// </summary>
        Enter = 4,

        /// <summary>
        /// Перемещение
        /// </summary>
        Move = 5,

        /// <summary>
        /// Инвентаризация
        /// </summary>
        Inventory = 6,

        /// <summary>
        /// Приходный ордер
        /// </summary>
        CashIn = 7,

        /// <summary>
        /// Расходный ордер
        /// </summary>
        CashOut = 8,

        /// <summary>
        /// Корректировка остатков в кассе
        /// </summary>
        CashboxAdjustment = 9,

        /// <summary>
        /// Корректировка остатков на счете
        /// </summary>
        AccountAdjustment = 10,

        /// <summary>
        /// Продажа
        /// </summary>
        RetailDemand = 11,

        /// <summary>
        /// Возврат
        /// </summary>
        RetailSalesReturn = 12,

        /// <summary>
        /// Смена
        /// </summary>
        RetailShift = 13,

        /// <summary>
        /// Внесение
        /// </summary>
        RetailDrawerCashIn = 14,

        /// <summary>
        /// Выплата
        /// </summary>
        RetailDrawerCashOut = 15
    }

    public interface IDocumentService : IDataService<Document>
    {
        event Action<DocumentView, DocumentTypeEnum> OnUpdated;
        event Action<DocumentView, DocumentTypeEnum> OnCreated;

        Task<Document> CreateAsync(Document entity, DocumentTypeEnum documentType);
        Task<Document> GetDocumentByIncludeAsync(int id);
        Task<DocumentView> GetDocumentViewById(int id);
        Task<IEnumerable<DocumentView>> GetDocumentViews(DocumentTypeEnum documentType);
        Task<string> GetNewNumber(DocumentTypeEnum documentType);
        Task<bool> CheckNumber(string number, DocumentTypeEnum documentType);
    }
}
