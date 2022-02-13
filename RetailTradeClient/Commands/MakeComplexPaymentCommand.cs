using DevExpress.XtraPrinting;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Properties;
using RetailTradeClient.Report;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RetailTradeClient.Commands
{
    public class MakeComplexPaymentCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly PaymentComplexViewModel _paymentComplexViewModel;
        private readonly IReceiptService _receiptService;
        private readonly IUIManager _manager;
        private readonly IShiftStore _shiftStore;
        private readonly IUserStore _userStore;

        #endregion

        #region Constructor

        public MakeComplexPaymentCommand(PaymentComplexViewModel paymentComplexViewModel,
            IReceiptService receiptService,
            IUIManager manager,
            IShiftStore shiftStore,
            IUserStore userStore)
        {
            _paymentComplexViewModel = paymentComplexViewModel;
            _receiptService = receiptService;
            _manager = manager;
            _shiftStore = shiftStore;
            _userStore = userStore;
        }

        #endregion

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if (_paymentComplexViewModel.Balance == 0)
            {
                try
                {
                    Receipt newReceipt;
                    //Создания чека
                    if (Settings.Default.IsKeepRecords)
                    {
                        newReceipt = await _receiptService.CreateAsync(new Receipt
                        {
                            DateOfPurchase = DateTime.Now,
                            Sum = _paymentComplexViewModel.AmountToBePaid,
                            PaidInCash = _paymentComplexViewModel.PaymentTypes.Where(pt => pt.Id == 1).Sum(pt => pt.Sum),
                            PaidInCashless = _paymentComplexViewModel.PaymentTypes.Where(pt => pt.Id == 2).Sum(pt => pt.Sum),
                            ShiftId = _shiftStore.CurrentShift.Id,
                            Change = 0,
                            ProductSales = _paymentComplexViewModel.SaleProducts.Select(s =>
                                new ProductSale
                                {
                                    ProductId = s.Id,
                                    Quantity = s.Quantity,
                                    Sum = s.Sum,
                                    SalePrice = s.SalePrice,
                                    ArrivalPrice = s.ArrivalPrice
                                }).ToList()
                        });
                    }
                    else
                    {
                        newReceipt = await _receiptService.SaleAsync(new Receipt
                        {
                            DateOfPurchase = DateTime.Now,
                            Sum = _paymentComplexViewModel.AmountToBePaid,
                            PaidInCash = _paymentComplexViewModel.PaymentTypes.Where(pt => pt.Id == 1).Sum(pt => pt.Sum),
                            PaidInCashless = _paymentComplexViewModel.PaymentTypes.Where(pt => pt.Id == 2).Sum(pt => pt.Sum),
                            ShiftId = _shiftStore.CurrentShift.Id,
                            Change = 0,
                            ProductSales = _paymentComplexViewModel.SaleProducts.Select(s =>
                                new ProductSale
                                {
                                    ProductId = s.Id,
                                    Quantity = s.Quantity,
                                    Sum = s.Sum,
                                    SalePrice = s.SalePrice,
                                    ArrivalPrice = s.ArrivalPrice
                                }).ToList()
                        });
                    }

                    //Подготовка документа для печати чека
                    ProductSaleReport report = new(_userStore, newReceipt)
                    {
                        DataSource = _paymentComplexViewModel.SaleProducts
                    };
                    await report.CreateDocumentAsync();

                    //Подготовка принтера
                    PrintToolBase tool = new(report.PrintingSystem);
                    tool.PrinterSettings.PrinterName = Properties.Settings.Default.DefaultReceiptPrinter;
                    tool.PrintingSystem.EndPrint += PrintingSystem_EndPrint;
                    tool.Print();


                }
                catch (Exception e)
                {
                    //ignore
                }
            }
            else
            {
                _manager.ShowMessage("Остаток должен быть 0", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            _manager.Close();
            _paymentComplexViewModel.Result = true;
        }
    }
}
