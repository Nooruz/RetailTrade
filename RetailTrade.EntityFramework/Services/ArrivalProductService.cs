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

        public event Action PropertiesChanged;
        public event Action<ArrivalProduct> OnEdited;
        public event Action<ArrivalProduct> OnCreated;

        public ArrivalProductService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            //_productService = productService;
            _nonQueryDataService = new NonQueryDataService<ArrivalProduct>(_contextFactory);
        }

        public async Task<ArrivalProduct> CreateAsync(ArrivalProduct entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                try
                {
                    //OnCreated?.Invoke(await GetByInclude(result.Id));
                    //Product product = await _productService.GetAsync(entity.ProductId);
                    //if (product != null)
                    //{
                    //    product.Quantity += result.Quantity;
                    //    _ = await _productService.UpdateAsync(product.Id, product);
                    //}
                }
                catch (Exception)
                {
                    //ignore
                }
            }
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
                OnEdited?.Invoke(result);
            return result;
        }

        public async Task<ArrivalProduct> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ArrivalProducts
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception)
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
            catch (Exception)
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
                    //Product product = await _productService.GetByIdAsync(item.ProductId);
                    //product.Quantity += item.Quantity;
                    //await _productService.UpdateAsync(product.Id, product);
                }
                return true;
            }
            catch (Exception)
            {
                //ignore
                return false;
            }            
        }

        public async Task<bool> EditAsync(ArrivalProduct newArrivalProduct)
        {
            try
            {
                ArrivalProduct oldArrivalProduct = await GetAsync(newArrivalProduct.Id);
                //Product product = await _productService.GetByIdAsync(oldArrivalProduct.ProductId);
                //if (product != null)
                //{
                //    product.Quantity += newArrivalProduct.Quantity - oldArrivalProduct.Quantity;
                //    _ = await _productService.UpdateAsync(product.Id, product);
                //}
                oldArrivalProduct.Quantity = newArrivalProduct.Quantity;
                oldArrivalProduct.ArrivalPrice = newArrivalProduct.ArrivalPrice;
                oldArrivalProduct.SalePrice = newArrivalProduct.SalePrice;
                oldArrivalProduct.ArrivalSum = newArrivalProduct.ArrivalSum;
                _ = await UpdateAsync(oldArrivalProduct.Id, oldArrivalProduct);
                return true;
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public async Task<ArrivalProduct> GetByInclude(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.ArrivalProducts
                    .Include(a => a.Product)
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> CreateRangeAsync(int wareHouseId, int arrivalId, IEnumerable<ArrivalProduct> arrivalProducts)
        {
            try
            {
                foreach (ArrivalProduct arrivalProduct in arrivalProducts)
                {
                    arrivalProduct.Product = null;
                    arrivalProduct.ArrivalId = arrivalId;
                    arrivalProduct.WareHouseId = wareHouseId;
                    var result = await _nonQueryDataService.Create(arrivalProduct);
                    OnCreated?.Invoke(result);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
