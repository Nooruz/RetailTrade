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
    public class ReceiptService : IReceiptService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Receipt> _nonQueryDataService;
        private readonly IRefundService _refundService;

        public event Action PropertiesChanged;

        public ReceiptService(RetailTradeDbContextFactory contextFactory,
            IRefundService refundService)
        {
            _contextFactory = contextFactory;
            _refundService = refundService;
            _nonQueryDataService = new NonQueryDataService<Receipt>(_contextFactory);
        }

        public async Task<Receipt> CreateAsync(Receipt entity)
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

        public async Task<Receipt> GetAsync(int id)
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                Receipt entity = await context.Receipts.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            await using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Receipt> entities = await context.Receipts.ToListAsync();
                return entities;
            }
        }

        public async Task<Receipt> UpdateAsync(int id, Receipt entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }

        public IEnumerable<Receipt> GetAll()
        {
            using (RetailTradeDbContext context = _contextFactory.CreateDbContext())
            {
                return context.Receipts.ToList();
            }
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsAsync()
        {
            await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            var result = await context.Receipts
                .Include(r => r.Shift)
                .ThenInclude(r => r.User)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsByDateAsync(DateTime dateTime)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                var result = await context.Receipts
                    .Where(r => r.DateOfPurchase.Date == dateTime.Date)
                    .Include(r => r.Shift)
                    .ThenInclude(r => r.User)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsFromCurrentShift(int shiftId)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                var result = await context.Receipts
                    .Where(r => r.ShiftId == shiftId)
                    .Include(r => r.ProductSales)
                    .ThenInclude(r => r.Product)
                    .ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> Refund(Receipt receipt)
        {
            try
            {
                _ = await _refundService.CreateAsync(new Refund
                {
                    DateOfRefund = DateTime.Now,
                    ShiftId = receipt.ShiftId,
                    Sum = receipt.Sum,
                    ProductRefunds = receipt.ProductSales.Select(p => new ProductRefund
                    {
                        Quantity = p.Quantity,
                        Sum = p.Sum,
                        SalePrice = p.SalePrice,
                        ProductId = p.ProductId
                    }).ToList()
                });

                await DeleteAsync(receipt.Id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return false;
        }
    }
}
