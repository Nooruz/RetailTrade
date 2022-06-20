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
    public class ProductBarcodeService : IProductBarcodeService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ProductBarcode> _nonQueryDataService;

        public ProductBarcodeService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<ProductBarcode>(_contextFactory);
        }

        public event Action<ProductBarcodeView> OnCreated;
        public event Action<ProductBarcode> OnEdited;
        public event Action<int> OnDeleted;

        public async Task<bool> CheckAsync(string barcode)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductBarcode.AnyAsync(p => p.Barcode == barcode);
            }
            catch (Exception)
            {
                //ignore
                return true;
            }
        }

        public async Task<ProductBarcode> CreateAsync(ProductBarcode entity)
        {
            await using var context = _contextFactory.CreateDbContext();
            ProductBarcode result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                OnCreated?.Invoke(await context.ProductBarcodeViews.FirstOrDefaultAsync(p => p.Id == result.Id));
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            bool result = await _nonQueryDataService.Delete(id);
            if (result)
            {
                OnDeleted?.Invoke(id);
            }
            return result;
        }

        public IEnumerable<ProductBarcode> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ProductBarcode
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductBarcode>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductBarcode
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductBarcodeView>> GetAllByProductIdAsync(int productId)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductBarcodeViews
                    .Where(p => p.ProductId == productId)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ProductBarcodeView>> GetAllViewsAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductBarcodeViews
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<ProductBarcode> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductBarcode
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<int> GetBarcodeCount(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ProductBarcode
                    .Where(p => p.ProductId == id).CountAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        public async Task<ProductBarcode> UpdateAsync(int id, ProductBarcode entity)
        {
            ProductBarcode result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
            {
                OnEdited?.Invoke(result);
            }
            return result;
        }
    }
}
