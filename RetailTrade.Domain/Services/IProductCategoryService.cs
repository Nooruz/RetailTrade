using RetailTrade.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductCategoryService : IDataService<ProductCategory>
    {
        IEnumerable<ProductCategory> GetAllIncludeProductSubcategory();
        Task<ObservableCollection<ProductCategory>> GetAllListAsync();

        event Action<ProductCategory> OnProductCategoryCreated;
        event Action<ProductCategory> OnProductCategoryUpdated;
    }
}
