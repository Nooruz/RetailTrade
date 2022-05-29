using RetailTrade.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTradeClient.State.ProductSales
{
    public interface IProductWareHouseService
    {
        Task<Product> GetProductByBarcode(string barcode);
        Task<IEnumerable<Product>> GetProducts();
    }
}
