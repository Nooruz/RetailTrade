using RetailTrade.Domain.Models;
using RetailTrade.Domain.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetailTrade.Domain.Services
{
    public interface IProductBarcodeService : IDataService<ProductBarcode>
    {
        Task<IEnumerable<ProductBarcodeView>> GetAllByProductIdAsync(int productId);
        Task<IEnumerable<ProductBarcodeView>> GetAllViewsAsync();
        Task RemoveRangeAsync(IEnumerable<ProductBarcode> productBarcodes);
        Task<int> GetBarcodeCount(int id);
        Task<bool> CheckAsync(string barcode);
        event Action<ProductBarcodeView> OnCreated;
        event Action<ProductBarcode> OnEdited;
        event Action<int> OnDeleted;
    }
}
