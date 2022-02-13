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
    public class RevaluationService : IRevaluationService
    {
        #region Private Members

        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Revaluation> _nonQueryDataService;
        private readonly IProductService _productService;

        #endregion

        #region Constructor

        public RevaluationService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Revaluation>(_contextFactory);
            _productService = new ProductService(_contextFactory);
        }

        #endregion

        public event Action PropertiesChanged;
        public event Action<Revaluation> OnRevaluationCreated;

        public async Task<Revaluation> CreateAsync(Revaluation entity)
        {
            if (entity.RevaluationProducts.Count != 0)
            {
                foreach (RevaluationProduct item in entity.RevaluationProducts)
                {
                    Product product = await _productService.GetAsync(item.ProductId);
                    product.ArrivalPrice = item.ArrivalPrice;
                    product.SalePrice = item.SalePrice;
                    await _productService.UpdateAsync(product.Id, product);
                }
            }
            var result = await _nonQueryDataService.Create(entity);            
            if (result != null)
                OnRevaluationCreated?.Invoke(result);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                var result = await _nonQueryDataService.Delete(id);
                if (result)
                {
                    PropertiesChanged?.Invoke();
                }
                return result;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public IEnumerable<Revaluation> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Revaluations
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Revaluation>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Revaluations
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Revaluation> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Revaluations
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Revaluation> UpdateAsync(int id, Revaluation entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
