using RetailTrade.Domain.Models;
using System.Collections.Generic;

namespace RetailTrade.Domain.Services
{
    public interface IProductCategoryService : IDataService<ProductCategory>
    {
        IEnumerable<ProductCategory> GetAllIncludeProductSubcategory();
        List<ProductCategory> GetAllList();
    }
}
