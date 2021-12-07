using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductSubcategoryService : IDataService<ProductSubcategory>
    {
        Task<IEnumerable<ProductSubcategory>> GetAllByProductCategoryIdAsync(int id);
        IEnumerable<ProductSubcategory> GetAllIncludeProductCategory();

        event Action<ProductSubcategory> OnProductSubcategoryCreated;
    }
}
