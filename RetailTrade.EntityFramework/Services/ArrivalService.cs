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
    public class ArrivalService : IArrivalService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Arrival> _nonQueryDataService;
        private readonly IProductService _productService;

        public ArrivalService(RetailTradeDbContextFactory contextFactory,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<Arrival>(_contextFactory);
        }

        public event Action PropertiesChanged;
        public event Action<Arrival> OnEdited;
        public event Action<Arrival> OnCreated;

        public async Task<bool> Clone(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Arrival arrival = await context.Arrivals
                    .Include(a => a.ArrivalProducts)
                    .FirstOrDefaultAsync(a => a.Id == id);

                await CreateAsync(new Arrival
                {
                    ArrivalDate = DateTime.Now,
                    ArrivalProducts = arrival.ArrivalProducts.Select(a =>
                        new ArrivalProduct
                        {
                            ProductId = a.ProductId,
                            Quantity = a.Quantity
                        }).ToList()
                });
            }
            catch (Exception e)
            {
                //ignore
            }

            return true;
        }

        public async Task<Arrival> CreateAsync(Arrival entity)
        {
            if (entity.ArrivalProducts.Any())
            {
                var result = await _nonQueryDataService.Create(entity);
                if (result != null)
                {
                    foreach (var item in entity.ArrivalProducts)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
                        product.Quantity += item.Quantity;
                        await _productService.UpdateAsync(product.Id, product);
                    }
                    OnCreated?.Invoke(await GetByIncludAsync(result.Id));
                }
            }                       
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Arrival arrival = await context.Arrivals
                    .Include(a => a.ArrivalProducts)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (arrival.ArrivalProducts.Count > 0)
                {
                    foreach (var item in arrival.ArrivalProducts)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
                        product.Quantity -= item.Quantity;
                        await _productService.UpdateAsync(product.Id, product);
                    }
                }

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

        public IEnumerable<Arrival> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Arrivals
                    .Include(o => o.ArrivalProducts)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Arrival>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Arrivals
                    .Include(o => o.ArrivalProducts)
                    .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Unit)
                    .Include(o => o.Supplier)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Arrival> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Arrivals
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Arrival> GetByIncludAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Arrivals
                    .Include(o => o.ArrivalProducts)
                    .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Unit)
                    .Include(o => o.Supplier)
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Arrival> UpdateAsync(int id, Arrival entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnEdited?.Invoke(await GetByIncludAsync(result.Id));
            return result;
        }
    }
}
