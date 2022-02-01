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
    public class OrderToSupplierService : IOrderToSupplierService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<OrderToSupplier> _nonQueryDataService;

        public OrderToSupplierService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<OrderToSupplier>(_contextFactory);
        }

        public event Action PropertiesChanged;
        public event Action<OrderToSupplier> OnOrderToSupplierCreated;

        public async Task<OrderToSupplier> CreateAsync(OrderToSupplier entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
                OnOrderToSupplierCreated?.Invoke(result);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _nonQueryDataService.Delete(id);
            if (result)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<OrderToSupplier> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.OrdersToSuppliers
                    .Include(o => o.OrderStatus)
                    .Include(o => o.OrderProducts)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<OrderToSupplier>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.OrdersToSuppliers
                    .Include(o => o.OrderStatus)
                    .Include(o => o.OrderProducts)
                    //.ThenInclude(o => o.Product)
                    .Include(o => o.Supplier)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<OrderToSupplier> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.OrdersToSuppliers
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<OrderToSupplier> UpdateAsync(int id, OrderToSupplier entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
