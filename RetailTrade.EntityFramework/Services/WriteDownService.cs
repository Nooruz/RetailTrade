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
    public class WriteDownService : IWriteDownService
    {
        private readonly RetailTradeDbContextFactory _contextFactory;
        private readonly NonQueryDataService<WriteDown> _nonQueryDataService;
        private readonly IProductService _productService;

        public WriteDownService(RetailTradeDbContextFactory contextFactory,
            IProductService productService)
        {
            _contextFactory = contextFactory;
            _productService = productService;
            _nonQueryDataService = new NonQueryDataService<WriteDown>(_contextFactory);
        }

        public event Action PropertiesChanged;

        public async Task<bool> Clone(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                WriteDown WriteDown = await context.WriteDowns
                    .Include(a => a.WriteDownProducts)
                    .FirstOrDefaultAsync(a => a.Id == id);

                List<WriteDownProduct> writeDownProducts = new();

                foreach (var item in WriteDown.WriteDownProducts)
                {
                    writeDownProducts.Add(new WriteDownProduct
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }

                WriteDown.Id = 0;
                WriteDown.WriteDownDate = DateTime.Now;
                WriteDown.WriteDownProducts = writeDownProducts;

                await CreateAsync(WriteDown);
            }
            catch (Exception e)
            {
                //ignore
            }

            return true;
        }

        public async Task<WriteDown> CreateAsync(WriteDown entity)
        {
            var result = await _nonQueryDataService.Create(entity);
            if (result != null)
            {
                if (entity.WriteDownProducts.Count > 0)
                {
                    foreach (var item in entity.WriteDownProducts)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
                        await _productService.UpdateAsync(product.Id, product);
                    }
                }
                PropertiesChanged?.Invoke();
            }
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                WriteDown WriteDown = await context.WriteDowns
                    .Include(a => a.WriteDownProducts)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (WriteDown.WriteDownProducts.Count > 0)
                {
                    foreach (var item in WriteDown.WriteDownProducts)
                    {
                        Product product = await _productService.GetByIdAsync(item.ProductId);
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

        public IEnumerable<WriteDown> GetAll()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                return context.WriteDowns
                    .Include(o => o.WriteDownProducts)
                    .ToList();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<IEnumerable<WriteDown>> GetAllAsync()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.WriteDowns
                    .Include(o => o.WriteDownProducts)
                    .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Unit)
                    .Include(o => o.Supplier)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<WriteDown> GetAsync(int id)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();
                return await context.WriteDowns
                    .FirstOrDefaultAsync((e) => e.Id == id);
            }
            catch (Exception e)
            {
                //ignore
            }
            return null;
        }

        public async Task<WriteDown> UpdateAsync(int id, WriteDown entity)
        {
            var result = await _nonQueryDataService.Update(id, entity);
            if (result != null)
                PropertiesChanged?.Invoke();
            return result;
        }
    }
}
