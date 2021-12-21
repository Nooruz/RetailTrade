using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductCategoryService : IDataService<ProductCategory>
    {
        IEnumerable<ProductCategory> GetAllIncludeProductSubcategory();
        Task<IEnumerable<ProductCategory>> GetAllListAsync();

        event Action<ProductCategory> OnProductCategoryCreated;
        event Action<ProductCategory> OnProductCategoryUpdated;
    }
}
