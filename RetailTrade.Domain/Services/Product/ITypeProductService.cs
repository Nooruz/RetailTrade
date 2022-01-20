using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface ITypeProductService : IDataService<TypeProduct>
    {
        /// <summary>
        /// Получить группы видов товара
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TypeProduct>> GetGroups();

        /// <summary>
        /// Получить видов товара
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TypeProduct>> GetTypes();

        event Action<TypeProduct> OnTypeProductCreated;
    }
}
