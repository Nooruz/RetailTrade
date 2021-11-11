using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Exceptions;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class ShiftService : IShiftService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Shift> _nonQueryDataService;

        public event Action PropertiesChanged;

        public ShiftService(RetailTradeDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Shift>(_contextFactory);
        }

        public async Task<Shift> CreateAsync(Shift entity)
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

        public async Task<Shift> GetAsync(int id)
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                Shift entity = await context.Shifts.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Shift>> GetAllAsync()
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Shift> entities = await context.Shifts.ToListAsync();
                return entities;
            }
        }

        public async Task<Shift> UpdateAsync(int id, Shift entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<Shift> GetAll()
        {
            using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Shift> entities = context.Shifts.ToList();
            return entities;
        }

        public async Task<Shift> OpeningShiftAsync(int userId)
        {
            try
            {
                var result = await _nonQueryDataService.Create(new Shift
                {
                    OpeningDate = DateTime.Now,
                    UserId = userId
                });
                if (result != null)
                {
                    PropertiesChanged?.Invoke();
                    return result;
                }
            }
            catch (ShiftException e)
            {
                throw new ShiftException(e.OpeningShiftDate, e.ClosingShiftDate, e.Message, e.InnerException);
            }
            return null;
        }

        public async Task<Shift> GetOpenShiftByUserIdAsync(int userId)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return await context.Shifts.FirstOrDefaultAsync(s => s.UserId == userId && s.ClosingDate == null);
            }
            catch (ShiftException e)
            {
                throw new ShiftException(e.OpeningShiftDate, e.ClosingShiftDate, e.Message, e.InnerException);
            }
        }

        public async Task<bool> ClosingShiftAsync(int userId)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                var result = await context.Shifts.FirstOrDefaultAsync(s => s.UserId == userId && s.ClosingDate == null);
                if (result != null)
                {
                    result.ClosingDate = DateTime.Now;
                    await UpdateAsync(result.Id, result);
                    return true;
                }
                return false;
            }
            catch (ShiftException e)
            {
                throw new ShiftException(e.OpeningShiftDate, e.ClosingShiftDate, e.Message, e.InnerException);
            }
        }

        public async Task<IEnumerable<Shift>> GetClosingShifts(DateTime startDate, DateTime endDate)
        {
            await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            return await context.Shifts.Where(s => s.ClosingDate != null && s.ClosingDate.Value.Date <= startDate.Date && s.ClosingDate.Value.Date >= endDate.Date)
                .Include(s => s.Receipts)
                .Select(s => new Shift { Id = s.Id, ClosingDate = s.ClosingDate, Receipts = 
                    s.Receipts.Select(r => new Receipt { Sum = r.Sum, PaidInCash = r.PaidInCash, PaidInCashless = r.PaidInCashless }) })
                .ToListAsync();
        }
    }
}
