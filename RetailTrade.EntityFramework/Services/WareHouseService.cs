using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class WareHouseService : IWareHouseService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<WareHouse> _nonQueryDataService;

        public WareHouseService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<WareHouse>(_contextFactory);
        }

        public event Action PropertiesChanged;
        public event Action<WareHouse> OnWareHouseCreated;
        public event Action<WareHouse> OnWareHouseEdited;

        public async Task<WareHouse> CreateAsync(WareHouse entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            OnWareHouseCreated?.Invoke(result);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<WareHouse> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.WareHouses
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<WareHouse>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.WareHouses
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<WareHouse> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.WareHouses
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<double> GetProductQuantityByProductId(int productId, int? wareHouseId, int documentId)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                if (wareHouseId != null && wareHouseId != 0)
                {
                    if (documentId != 0)
                    {
                        return await context.ProductStockViews.Where(p => p.ProductId == productId && p.WareHouseId == wareHouseId && p.DocumentId < documentId).SumAsync(p => p.Quantity);
                    }
                    return await context.ProductStockViews.Where(p => p.ProductId == productId && p.WareHouseId == wareHouseId).SumAsync(p => p.Quantity);
                }
                if (documentId != 0)
                {
                    return await context.ProductStockViews.Where(p => p.ProductId == productId && p.DocumentId < documentId).SumAsync(p => p.Quantity);
                }
                return await context.ProductStockViews.Where(p => p.ProductId == productId).SumAsync(p => p.Quantity);
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        public async Task<IEnumerable<ProductStockView>> GetProductStockByProductId(int productId)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductStockViews.Where(p => p.ProductId == productId).ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task MarkingForDeletion(WareHouse wareHouse)
        {
            try
            {
                wareHouse.DeleteMark = !wareHouse.DeleteMark;
                _ = await UpdateAsync(wareHouse.Id, wareHouse);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public async Task<WareHouse> UpdateAsync(int id, WareHouse entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnWareHouseEdited?.Invoke(result);
            return result;
        }
    }
}
