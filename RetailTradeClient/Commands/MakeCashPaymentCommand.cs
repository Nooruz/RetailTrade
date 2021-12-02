﻿using DevExpress.XtraPrinting;
using DrvFRLib;
using RetailTrade.CashRegisterMachine;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Report;
using RetailTradeClient.State.CashRegisterControlMachine;
using RetailTradeClient.State.Dialogs;
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
        private readonly IProductSaleService _productSaleService;
        private readonly IUIManager _manager;
        private readonly IShiftStore _shiftStore;
        private readonly IUserStore _userStore;
        private DrvFR _cashRegisterControlMachine;

        #endregion

        #region Constructor

        public MakeCashPaymentCommand(PaymentCashViewModel paymentCashViewModel,
            IReceiptService receiptService,
            IProductSaleService productSaleService,
            IUIManager manager,
            ICashRegisterControlMachine cashRegisterControlMachine,
            IShiftStore shiftStore,
            IUserStore userStore)
        {
            _paymentCashViewModel = paymentCashViewModel;
            _receiptService = receiptService;
            _productSaleService = productSaleService;
            _manager = manager;
            _cashRegisterControlMachine = cashRegisterControlMachine.GetCashRegisterControlMachine();
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
            if (_paymentCashViewModel.Change >= 0)
            {
                try
                {
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

                    ShtrihM.Connect();
                    ShtrihM.CheckType = 0;

                    if (newReceipt != null)
                    {
                        foreach (Sale sale in _paymentCashViewModel.SaleProducts)
                        {

                            ShtrihM.Quantity = Convert.ToDouble(sale.Quantity);
                            ShtrihM.Price = sale.SalePrice;
                            var sum1NSP = Math.Round(sale.SalePrice * 1 / 113, 2);
                            var sum1NDS = Math.Round(sale.SalePrice * 12 / 113, 2);
                            string sumNSP = Math.Round(sum1NSP * 100, 0).ToString();
                            string sumNDS = Math.Round(sum1NDS * 100, 0).ToString();

                            ShtrihM.StringForPrinting =
                                string.Join(";", new string[] { "0", "12345", "0", "0", "2", sumNDS, "3", sumNSP + "\n" + sale.Name });

                            ShtrihM.BarCode = "46198488";

                            ShtrihM.Tax1 = 2;
                            ShtrihM.Tax2 = 3;
                            ShtrihM.Tax3 = 1;
                            ShtrihM.Tax4 = 4;

                            ShtrihM.Sale();
                        }
                        ShtrihM.Summ1 = newReceipt.Sum;
                        ShtrihM.StringForPrinting = "";
                        ShtrihM.CloseCheck();
                        ShtrihM.CutCheck();
                    }

                    //Подготовка документа для печати чека
                    ProductSaleReport report = new(_userStore, newReceipt)
                    {
                        DataSource = _paymentCashViewModel.SaleProducts
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
                finally
                {
                    ShtrihM.Disconnect();
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
