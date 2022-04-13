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
    public class CustomerService : ICustomerService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Customer> _nonQueryDataService;

        public CustomerService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Customer>(_contextFactory);
        }

        public event Action PropertiesChanged;
        public event Action<Customer> OnEdited;
        public event Action<Customer> OnCreated;

        public async Task<Customer> CreateAsync(Customer entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await _nonQueryDataService.Delete(id);
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public IEnumerable<Customer> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Customers
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Customers
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Customer> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Customers
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Customer> UpdateAsync(int id, Customer entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                OnEdited?.Invoke(result);
            return result;
        }
    }
}
