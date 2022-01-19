using DevExpress.XtraPrinting;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Properties;
using RetailTradeClient.Report;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.ProductSale;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RetailTradeClient.Commands
{
    public class MakeCashPaymentCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly PaymentCashViewModel _paymentCashViewModel;
        private readonly IReceiptService _receiptService;
        private readonly IUIManager _manager;
        private readonly IShiftStore _shiftStore;
        private readonly IUserStore _userStore;
        private readonly IProductSaleStore _productSaleStore;

        #endregion

        #region Constructor

        public MakeCashPaymentCommand(PaymentCashViewModel paymentCashViewModel,
            IReceiptService receiptService,
            IUIManager manager,
            IShiftStore shiftStore,
            IUserStore userStore,
            IProductSaleStore productSaleStore)
        {
            _paymentCashViewModel = paymentCashViewModel;
            _receiptService = receiptService;
            _manager = manager;
            _shiftStore = shiftStore;
            _userStore = userStore;
            _productSaleStore = productSaleStore;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (_paymentCashViewModel.Change >= 0)
            {
                try
                {
                    var r = _productSaleStore.ProductSales;
                    //Создания чека
                    Receipt newReceipt = await _receiptService.CreateAsync(new Receipt
                    {
                        DateOfPurchase = DateTime.Now,
                        Sum = _paymentCashViewModel.AmountToBePaid,
                        PaidInCash = _paymentCashViewModel.Entered,
                        ShiftId = _shiftStore.CurrentShift.Id,
                        Change = _paymentCashViewModel.Change,
                        ProductSales = _paymentCashViewModel.SaleProducts.Select(s =>
                            new ProductSale
                            {
                                ProductId = s.Id,
                                Quantity = s.Quantity,
                                Sum = s.Sum,
                                SalePrice = s.SalePrice,
                                ArrivalPrice = s.ArrivalPrice
                            }).ToList()
                    });

                    if (Settings.Default.ShtrihMConnected)
                    {
                        ShtrihM.Connect();
                        ShtrihM.CheckType = 0;

                        if (newReceipt != null)
                        {
                            foreach (Sale sale in _paymentCashViewModel.SaleProducts)
                            {
                                ShtrihM.Password = 30;
                                ShtrihM.Department = 1;
                                ShtrihM.Quantity = Convert.ToDouble(sale.Quantity);
                                ShtrihM.Price = sale.SalePrice;
                                var sum1NSP = Math.Round(sale.SalePrice * 1 / 102, 2);
                                //var sum1NDS = Math.Round(sale.SalePrice * 12 / 113, 2);
                                string sumNSP = Math.Round(sum1NSP * 100, 0).ToString();
                                //string sumNDS = Math.Round(sum1NDS * 100, 0).ToString();

                                ShtrihM.StringForPrinting =
                                    string.Join(";", new string[] { "", sale.TNVED, "", "", "0", "", "4", sumNSP + "\n" + sale.Name });

                                //ShtrihM.BarCode = "46198488";

                                ShtrihM.Tax1 = 0;
                                ShtrihM.Tax2 = 4;
                                ShtrihM.Tax3 = 0;
                                ShtrihM.Tax4 = 0;

                                ShtrihM.Sale();
                            }
                            ShtrihM.Summ1 = newReceipt.Sum;
                            ShtrihM.StringForPrinting = "";
                            ShtrihM.CloseCheck();
                            ShtrihM.CutCheck();
                        }
                    }                    

                    //Подготовка документа для печати чека
                    ProductSaleReport report = new(_userStore, newReceipt)
                    {
                        DataSource = _paymentCashViewModel.SaleProducts
                    };
                    await report.CreateDocumentAsync();

                    //Подготовка принтера
                    PrintToolBase tool = new(report.PrintingSystem);
                    tool.PrinterSettings.PrinterName = Settings.Default.DefaultReceiptPrinter;
                    tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
                    tool.Print();


                }
                catch (Exception e)
                {
                    //ignore
                }
                finally
                {
                    //ShtrihM.Disconnect();
                }
            }
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            _manager.Close();
            _paymentCashViewModel.Result = true;
        }
    }
}
