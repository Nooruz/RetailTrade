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
    public class SupplierService : ISupplierService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Supplier> _nonQueryDataService;

        public event Action PropertiesChanged;

        public SupplierService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Supplier>(_contextFactory);
        }

        public async Task<Supplier> CreateAsync(Supplier entity)
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

        public async Task<Supplier> GetAsync(int id)
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                Supplier entity = await context.Suppliers.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Supplier> entities = await context.Suppliers.ToListAsync();
                return entities;
            }
        }

        public async Task<Supplier> UpdateAsync(int id, Supplier entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<Supplier> GetAll()
        {
            using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Supplier> entities = context.Suppliers.ToList();
                return entities;
            }
        }

        public IEnumerable<Supplier> GetOnlyNames()
        {
            using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Supplier> entities = context.Suppliers.Select(s => new Supplier { Id = s.Id, ShortName = s.ShortName }).ToList();
                return entities;
            }
        }
    }
}
