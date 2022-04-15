﻿using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTrade.EntityFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RetailTrade.EntityFramework.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Receipt> _nonQueryDataService;
        private readonly IRefundService _refundService;
        private readonly IProductService _productService;

        public event Action PropertiesChanged;
        public event Action<IEnumerable<ProductSale>> OnProductSale;

        public ReceiptService(RetailTradeDbContextFactory contextFactory,
            IRefundService refundService,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _refundService = refundService;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<Receipt>(_contextFactory);            
        }

        public async Task<Receipt> CreateAsync(Receipt entity)
        {
            try
            {
                var result = await _nonQueryDataService.Create(entity);
                foreach (var productSale in entity.ProductSales)
                {
                    _ = await _productService.Sale(productSale.ProductId, productSale.Quantity, true);
                }
                if (result != null)
                    OnProductSale?.Invoke(entity.ProductSales);
                return result;
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
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
            await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            Receipt entity = await context.Receipts.FirstOrDefaultAsync((e) => e.Id == id);
            return entity;
        }

        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            IEnumerable<Receipt> entities = await context.Receipts.ToListAsync();
            return entities;
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
            using RetailTradeDbContext context = _contextFactory.CreateDbContext();
            return context.Receipts.ToList();
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsFromCurrentShiftAsync(int shiftId)
        {
            try
            {
                await using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(r => r.ShiftId == shiftId)
                    .Include(r => r.ProductSales)
                    .ThenInclude(r => r.Product)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public IEnumerable<Receipt> GetReceiptsFromCurrentShift(int shiftId)
        {
            try
            {
                using RetailTradeDbContext context = _contextFactory.CreateDbContext();
                return context.Receipts
                    .Where(r => r.ShiftId == shiftId)
                    .Include(r => r.ProductSales)
                    .ThenInclude(r => r.Product)
                    .ToList();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<bool> Refund(Receipt receipt)
        {
            try
            {
                Refund result = await _refundService.CreateAsync(new Refund
                {
                    DateOfRefund = DateTime.Now,
                    ShiftId = receipt.ShiftId,
                    Sum = receipt.Sum,
                    ProductRefunds = receipt.ProductSales.Select(p => new ProductRefund
                    {
                        Quantity = p.Quantity,
                        Sum = p.Total,
                        SalePrice = p.SalePrice,
                        ProductId = p.ProductId
                    }).ToList()
                });
                if (result != null)
                {
                    receipt.IsRefund = true;
                    receipt.ProductSales = null;
                    await UpdateAsync(receipt.Id, receipt);
                    return true;
                }                
            }
            catch (Exception)
            {
                //ignore
            }
            return false;
        }

        public async Task<IEnumerable<Receipt>> Predicate(Expression<Func<Receipt, bool>> predicate, Expression<Func<Receipt, Receipt>> select)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(predicate)
                    .Include(r => r.ProductSales)
                    .Include(r => r.Shift)
                    .ThenInclude(r => r.User)
                    .Select(select)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Receipt>> GetSaleAmoundToday()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(r => r.IsRefund == false && r.DateOfPurchase.Date == DateTime.Now.Date)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Receipt>> GetSaleAmoundYesterday()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(r => r.IsRefund == false && r.DateOfPurchase.Date == DateTime.Now.Date.AddDays(-1))
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Receipt>> GetSaleAmoundLastWeek()
        {
            try
            {
                DateTime mondayOfLastWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
                await using var context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(r => r.IsRefund == false && r.DateOfPurchase.Date >= mondayOfLastWeek && r.DateOfPurchase <= mondayOfLastWeek.AddDays(6))
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<Receipt>> GetSaleAmoundCurrentMonth()
        {
            try
            {
                DateTime firstDayOfCurrentMonth = new(DateTime.Now.Year, DateTime.Now.Month, 1);
                await using var context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(r => r.IsRefund == false && r.DateOfPurchase.Date >= firstDayOfCurrentMonth)
                    .ToListAsync();
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<decimal> GetSaleAmoundLastMonth()
        {
            try
            {
                DateTime firstDayOfLastMonth = new(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1);
                await using var context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(r => r.IsRefund == false && r.DateOfPurchase.Date >= firstDayOfLastMonth && r.DateOfPurchase <= firstDayOfLastMonth.AddMonths(1).AddDays(-1))
                    .SumAsync(s => s.Sum);
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        public async Task<decimal> GetSaleAmoundBeginningYear()
        {
            try
            {
                DateTime beginningYear = new(DateTime.Now.Year, 1, 1);
                await using var context = _contextFactory.CreateDbContext();
                return await context.Receipts
                    .Where(r => r.IsRefund == false && r.DateOfPurchase.Date >= beginningYear)
                    .SumAsync(s => s.Sum);
            }
            catch (Exception)
            {
                //ignore
            }
            return 0;
        }

        public async Task SetKKMCheckNumber(int id, string checkNumber)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                Receipt receipt = await GetAsync(id);
                receipt.KKMCheckNumber = checkNumber;
                await UpdateAsync(id, receipt);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public async Task<Receipt> SaleAsync(Receipt receipt)
        {
            try
            {
                return await _nonQueryDataService.Create(receipt);
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }

        public async Task<Receipt> CreateAsync(Receipt receipt, bool isKeepRecords)
        {
            try
            {
                var result = await _nonQueryDataService.Create(receipt);
                foreach (var productSale in receipt.ProductSales)
                {
                    _ = await _productService.Sale(productSale.ProductId, productSale.Quantity, isKeepRecords);
                }
                if (result != null)
                {
                    if (isKeepRecords)
                    {
                        OnProductSale?.Invoke(receipt.ProductSales);
                    }
                    else
                    {
                        PropertiesChanged?.Invoke();
                    }
                }
                return result;
            }
            catch (Exception)
            {
                //ignore
            }
            return null;
        }
    }
}
