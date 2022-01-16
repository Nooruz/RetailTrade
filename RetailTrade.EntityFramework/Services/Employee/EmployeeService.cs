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
    public class EmployeeService : IEmployeeService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Employee> _nonQueryDataService;

        public EmployeeService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Employee>(_contextFactory);
        }

        public event Action PropertiesChanged;

        public async Task<Employee> CreateAsync(Employee entity)
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

        public async Task<Employee> UpdateAsync(int id, Employee entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<Employee> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.Employees
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Employees
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<Employee> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Employees
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }
    }
}
