using DevExpress.XtraPrinting;
using DrvFRLib;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeClient.Report;
using RetailTradeClient.State.CashRegisterControlMachine;
using RetailTradeClient.State.Dialogs;
using RetailTradeClient.State.Shifts;
using RetailTradeClient.State.Users;
using RetailTradeClient.ViewModels.Dialogs;
using System;
using System.Threading.Tasks;

namespace RetailTradeClient.Commands
{
    public class MakeCashPaymentCommand : AsyncCommandBase
    {
        #region Private Members

        private readonly PaymentCashViewModel _viewModel;
        private readonly IReceiptService _receiptService;
        private readonly IDataService<ProductSale> _productSaleService;
        private readonly IUIManager _manager;
        private readonly IShiftStore _shiftStore;
        private readonly IUserStore _userStore;
        private DrvFR _cashRegisterControlMachine;

        #endregion

        #region Constructor

        public MakeCashPaymentCommand(PaymentCashViewModel viewModel,
            IReceiptService receiptService,
            IDataService<ProductSale> productSaleService,
            IUIManager manager,
            ICashRegisterControlMachine cashRegisterControlMachine,
            IShiftStore shiftStore,
            IUserStore userStore)
        {
            _viewModel = viewModel;
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
            if (_viewModel.Change >= 0)
            {
                try
                {                    
                    //Создания чека
                    Receipt newReceipt = await _receiptService.CreateAsync(new Receipt
                    {
                        DateOfPurchase = DateTime.Now,
                        Sum = _viewModel.AmountToBePaid,
                        PaidInCash = _viewModel.Entered,
                        ShiftId = _shiftStore.CurrentShift.Id,
                        Change = _viewModel.Change
                    });

                    _cashRegisterControlMachine.Connect();
                    _cashRegisterControlMachine.OpenCheck();

                    if (newReceipt != null)
                    {
                        foreach (Sale sale in _viewModel.SaleProducts)
                        {
                            _ = await _productSaleService.CreateAsync(new ProductSale
                            {
                                ProductId = sale.Id,
                                Quantity = sale.Quantity,
                                Sum = sale.Sum,
                                ReceiptId = newReceipt.Id
                            });

                            _cashRegisterControlMachine.Quantity = Convert.ToDouble(sale.Quantity);
                            _cashRegisterControlMachine.Price = sale.SalePrice;
                            var sum1NSP = Math.Round(sale.SalePrice * 1 / 113, 2);
                            var sum1NDS = Math.Round(sale.SalePrice * 12 / 113, 2);
                            string sumNSP = Math.Round(sum1NSP * 100, 0).ToString();
                            string sumNDS = Math.Round(sum1NDS * 100, 0).ToString();
                            _cashRegisterControlMachine.StringForPrinting = 
                                string.Join(",", new string[] { "", "", "", "", "2", sumNDS, "3", sumNSP + Environment.NewLine + sale.Name });
                            _cashRegisterControlMachine.Sale();
                        }
                        _cashRegisterControlMachine.Summ1 = newReceipt.Sum;
                        _cashRegisterControlMachine.CloseCheck();
                        _cashRegisterControlMachine.CutCheck();
                    }

                    //Подготовка документа для печати чека
                    ProductSaleReport report = new(_userStore, newReceipt)
                    {
                        DataSource = _viewModel.SaleProducts
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
                    _cashRegisterControlMachine.Disconnect();
                }              
            }
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            _manager.Close();
            _viewModel.Result = true;
        }
    }
}
