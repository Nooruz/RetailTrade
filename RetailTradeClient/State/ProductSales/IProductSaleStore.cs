using RetailTrade.Domain.Models;
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
        event Action<bool> OnProductSale;
        event Action OnPostponeReceiptChanged;

        Task AddProduct(string barcode);
        Task AddProduct(int id);
        void DeleteProduct(int id);
        void ProductSale(bool success);
        void ProductSaleCashless(bool success);
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
