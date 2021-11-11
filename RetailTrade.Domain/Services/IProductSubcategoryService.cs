using RetailTrade.Domain.Models;
using System.Collections.Generic;

namespace RetailTrade.Domain.Services
{
    public interface IProductSubcategoryService : IDataService<ProductSubcategory>
    {
        IEnumerable<ProductSubcategory> GetAllByProductCategoryId(int id);
        IEnumerable<ProductSubcategory> GetAllIncludeProductCategory();
    }
}
