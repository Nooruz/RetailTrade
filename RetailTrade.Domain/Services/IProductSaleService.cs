using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductSaleService : IDataService<ProductSale>
    {
        /// <summary>
        /// Получить все проданные товары за период
        /// </summary>
        /// <param name="startDate">Дата начало</param>
        /// <param name="endDate">Дата окончание</param>
        /// <returns></returns>
        Task<IEnumerable<ProductSale>> GetProductSalesByDateRange(DateTime startDate, DateTime endDate);

        Task<IEnumerable<ProductSale>> GetRatingTenProducts();
    }
}
