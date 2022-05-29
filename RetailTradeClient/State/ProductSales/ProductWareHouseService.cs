using Microsoft.EntityFrameworkCore;
using RetailTrade.Domain.Models;
using RetailTrade.EntityFramework;
using RetailTradeClient.State.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTradeClient.State.ProductSales
{
    public class ProductWareHouseService : IProductWareHouseService
    {
        private readonly IUserStore _userStore;
        private readonly RetailTradeDbContextFactory _contextFactory;

        public ProductWareHouseService(RetailTradeDbContextFactory contextFactory,
            IUserStore userStore)
        {
            _contextFactory = contextFactory;
            _userStore = userStore;
        }

        public async Task<Product> GetProductByBarcode(string barcode)
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();

                Product product = await context.Products.FirstOrDefaultAsync(p => p.DeleteMark == false && p.Barcode == barcode);

                if (product != null)
                {
                    ProductWareHouse productWareHouse = await context.ProductsWareHouses
                        .FirstOrDefaultAsync(pw => pw.ProductId == product.Id && pw.WareHouseId == _userStore.CurrentUser.WareHouseId);
                    if (productWareHouse != null)
                    {
                        product.Quantity = productWareHouse.Quantity;
                    }
                }

                return product;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                await using var context = _contextFactory.CreateDbContext();

                return await context.Products                    
                    .Include(w => w.ProductsWareHouses.Where(pw => pw.WareHouseId == _userStore.CurrentUser.WareHouseId && pw.Quantity > 0))
                    .Where(p => p.DeleteMark == false && p.ProductsWareHouses.FirstOrDefault(pw => pw.Quantity > 0) != null)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
