using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class RevaluationProductService : IRevaluationProductService
    {
        #region Private Members

        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<RevaluationProduct> _nonQueryDataService;
        private readonly IProductService _productService;

        #endregion

        #region Constructor

        public RevaluationProductService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<RevaluationProduct>(_contextFactory);
            _productService = new ProductService(_contextFactory);
        }

        #endregion

        public event Action PropertiesChanged;
        public event Action<RevaluationProduct> OnRevaluationCreated;

        public async Task<RevaluationProduct> CreateAsync(RevaluationProduct entity)
        {
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

        public IEnumerable<RevaluationProduct> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.RevaluationProducts
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<RevaluationProduct>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.RevaluationProducts
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<RevaluationProduct>> GetAllByRevaluationId(int revaluationId)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.RevaluationProducts
                    .Where(r => r.RevaluationId == revaluationId)
                    .Include(r => r.Product)
                    .ThenInclude(r => r.Unit)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<RevaluationProduct> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.RevaluationProducts
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<RevaluationProduct> UpdateAsync(int id, RevaluationProduct entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
