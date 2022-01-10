using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class ProductSubcategoryService : IProductSubcategoryService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ProductSubcategory> _nonQueryDataService;

        public event Action PropertiesChanged;
        public event Action<ProductSubcategory> OnProductSubcategoryCreated;
        public event Action<ProductSubcategory> OnProductSubcategoryUpdated;

        public ProductSubcategoryService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<ProductSubcategory>(_contextFactory);
        }

        public async Task<ProductSubcategory> CreateAsync(ProductSubcategory entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                OnProductSubcategoryCreated?.Invoke(result);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<ProductSubcategory> UpdateAsync(int id, ProductSubcategory entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnProductSubcategoryUpdated?.Invoke(result);
            return result;
        }

        public async Task<ProductSubcategory> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductSubcategories
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<ProductSubcategory> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ProductSubcategories
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductSubcategory>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductSubcategories
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<ProductSubcategory> GetFirstOrDefaultAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductSubcategories.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductSubcategory>> GetAllByProductCategoryIdAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductSubcategories
                    .Where(ps => ps.ProductCategoryId == id)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<ProductSubcategory> GetAllIncludeProductCategory()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                var list = context.ProductSubcategories
                    .Include(ps => ps.ProductCategory)
                    .ToList();
                return list;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }
    }
}
