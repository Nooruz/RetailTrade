using RetailTrade.Domain.Models;
using RetailTradeClient.ViewModels.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RetailTradeClient.State.ProductSales
{
    public interface IProductSaleStore
    {
        ObservableCollection<Sale> Sales { get; }
        ObservableCollection<PostponeReceipt> PostponeReceipts { get; }
        ObservableCollection<PaymentType> PaymentTypes { get; set; }
        decimal ToBePaid { get; }
        decimal Entered { get; set; }
        decimal Change { get; set; }
        public bool SaleCompleted { get; set; }
        event Action OnProductSalesChanged;
        event Action<decimal> OnProductSale;
        event Action OnPostponeReceiptChanged;
        event Action<Sale> OnCreated;

        ProductViewModel SearchProduct();
        Task AddProduct(string barcode);
        Task AddProduct(int id);
        void DeleteProduct(int id);
        void ChangingQuantity(int id, double quantity);
        Task CashPayment();
        Task CashlessPayment();
        /// <summary>
        /// Отложить чек
        /// </summary>
        /// <returns></returns>
        void CreatePostponeReceipt();
        void ResumeReceipt(Guid guid);
    }
}
