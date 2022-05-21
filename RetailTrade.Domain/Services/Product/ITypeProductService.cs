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
        Task<IEnumerable<TypeProduct>> GetGroupsAsync();

        /// <summary>
        /// Получить видов товара
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TypeProduct>> GetTypesAsync();

        Task<bool> CanDelete(TypeProduct typeProduct);

        event Action<TypeProduct> OnTypeProductCreated;
        event Action<TypeProduct> OnTypeProductEdited;
    }
}
