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
    public class ArrivalProductService : IArrivalProductService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ArrivalProduct> _nonQueryDataService;
        private readonly IProductService _productService;

        public event Action PropertiesChanged;

        public ArrivalProductService(RetailTradeDbContextFactory contextFactory, 
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<ArrivalProduct>(_contextFactory);
        }

        public async Task<ArrivalProduct> CreateAsync(ArrivalProduct entity)
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

        public async Task<ArrivalProduct> UpdateAsync(int id, ArrivalProduct entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public async Task<ArrivalProduct> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ArrivalProducts
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<ArrivalProduct> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.ArrivalProducts
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<ArrivalProduct>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ArrivalProducts
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> AddRangeAsync(List<ArrivalProduct> arrivalProducts)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                foreach (ArrivalProduct item in arrivalProducts)
                {
                    await CreateAsync(new ArrivalProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                    Product product = await _productService.GetByIdAsync(item.ProductId);
                    product.Quantity += item.Quantity;
                    await _productService.UpdateAsync(product.Id, product);
                }
                return true;
            }
            catch (Exception e)
            {
                //ignore
                return false;
            }            
        }
    }
}
