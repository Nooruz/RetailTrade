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
                    _cashRegisterControlMachine.CheckType = 0;

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
                                string.Join(";", new string[] { "0", "5484654", "48574", "0", "2", sumNDS, "3", sumNSP, Environment.NewLine + sale.Name });

                            _cashRegisterControlMachine.BarCode = "46198488";

                            _cashRegisterControlMachine.Tax1 = 2;
                            _cashRegisterControlMachine.Tax2 = 3;
                            _cashRegisterControlMachine.Tax3 = 1;
                            _cashRegisterControlMachine.Tax4 = 4;

                            _cashRegisterControlMachine.Sale();
                        }
                        _cashRegisterControlMachine.Summ1 = newReceipt.Sum;
                        _cashRegisterControlMachine.StringForPrinting = "";
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

        private void Test()
        {
            _cashRegisterControlMachine.CheckType = 0;
            _cashRegisterControlMachine.Connect();
            _cashRegisterControlMachine.OpenCheck();

            /*      _cashRegisterControlMachine.TagNumber = 1261;   //Доп. реквизит пользователя
                  _cashRegisterControlMachine.FNBeginSTLVTag();

                  int my_TagID = _cashRegisterControlMachine.TagID;

                  _cashRegisterControlMachine.TagID = my_TagID;
                  _cashRegisterControlMachine.TagNumber = 1262;
                  _cashRegisterControlMachine.TagType = 7;
                  _cashRegisterControlMachine.TagValueStr = "020";
                  _cashRegisterControlMachine.FNAddTag();

                  _cashRegisterControlMachine.TagID = my_TagID;
                  _cashRegisterControlMachine.TagNumber = 1263;
                  _cashRegisterControlMachine.TagType = 7;
                  _cashRegisterControlMachine.TagValueStr = "14.12.2018";
                  _cashRegisterControlMachine.FNAddTag();

                  _cashRegisterControlMachine.TagID = my_TagID;
                  _cashRegisterControlMachine.TagNumber = 1264;
                  _cashRegisterControlMachine.TagType = 7;
                  _cashRegisterControlMachine.TagValueStr = "1556";
                  _cashRegisterControlMachine.FNAddTag();

                  _cashRegisterControlMachine.TagID = my_TagID;
                  _cashRegisterControlMachine.TagNumber = 1265;
                  _cashRegisterControlMachine.TagType = 7;
                  _cashRegisterControlMachine.TagValueStr = "tm=mdlp&sid00000000113101";
                  _cashRegisterControlMachine.FNAddTag();

                     _cashRegisterControlMachine.FNSendSTLVTag();
            */
            _cashRegisterControlMachine.BarCode = "010460702776893521000000013JBSF91FFD092dGVzdGifC5FkjETjJhotf7m8rsjQHeoNyxcpaEIZfDQ=";
            //организация работы с мерным кол-вом (теги 1291, 1292, 1293 и 1294)

            _cashRegisterControlMachine.Quantity = 1;
            _cashRegisterControlMachine.Price = 90;
            _cashRegisterControlMachine.Tax1 = 1;
            _cashRegisterControlMachine.Department = 1;
                                         //организация передачи наименования товарной позиции. Мах длинна не более 128 сим.
            _cashRegisterControlMachine.StringForPrinting = "Л-тироксин 50 Берлин-Хеми тб 50мкг N 50"; // данное наименование товара на чеке печатается и передается в ОФД.

            _cashRegisterControlMachine.BarCode = "010460702776893521000000013JBSF91FFD092dGVzdGifC5FkjETjJhotf7m8rsjQHeoNyxcpaEIZfDQ=";
            _cashRegisterControlMachine.Summ1 = 2000;
            _cashRegisterControlMachine.Summ2 = 0;
            _cashRegisterControlMachine.Summ3 = 0;
            _cashRegisterControlMachine.Summ4 = 0;
            _cashRegisterControlMachine.Summ5 = 0;
            _cashRegisterControlMachine.Summ6 = 0;
            _cashRegisterControlMachine.Summ7 = 0;
            _cashRegisterControlMachine.Summ8 = 0;
            _cashRegisterControlMachine.Summ9 = 0;
            _cashRegisterControlMachine.Summ10 = 0;
            _cashRegisterControlMachine.Summ11 = 0;
            _cashRegisterControlMachine.Summ12 = 0;
            _cashRegisterControlMachine.Summ13 = 0;
            _cashRegisterControlMachine.Summ14 = 0;
            _cashRegisterControlMachine.Summ15 = 0;
            _cashRegisterControlMachine.Summ16 = 0;
            //      _cashRegisterControlMachine.TaxType = 1;
            _cashRegisterControlMachine.StringForPrinting = "=================================================================";
            _cashRegisterControlMachine.CloseCheck();
            _cashRegisterControlMachine.CutCheck();
        }

        private void PrintingSystem_EndPrint(object sender, EventArgs e)
        {
            _manager.Close();
            _viewModel.Result = true;
        }
    }
}
