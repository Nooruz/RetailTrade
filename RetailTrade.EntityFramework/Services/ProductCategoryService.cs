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
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ProductCategory> _nonQueryDataService;

        public event Action PropertiesChanged;

        public ProductCategoryService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<ProductCategory>(_contextFactory);
        }

        public async Task<ProductCategory> CreateAsync(ProductCategory entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<ProductCategory> UpdateAsync(int id, ProductCategory entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<ProductCategory> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductCategories
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ProductCategories
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductCategories
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<ProductCategory> GetFirstOrDefaultAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductCategories.FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<ProductCategory> GetAllIncludeProductSubcategory()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ProductCategories
                    .Include(pc => pc.ProductSubcategories)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public List<ProductCategory> GetAllList()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                List<ProductCategory> productCategories = new List<ProductCategory>
                {
                    new ProductCategory { Id = 0, Name = "Все категории" }
                };
                productCategories.AddRange(context.ProductCategories.Include(pc => pc.ProductSubcategories).ToList());
                return productCategories;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }
    }
}
