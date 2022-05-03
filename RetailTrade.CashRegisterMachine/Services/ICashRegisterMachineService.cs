using System;

namespace RetailTrade.CashRegisterMachine.Services
{
    public enum CashRegisterType
    {
        OkaMF,
        ShtrihM
    }

    public interface ICashRegisterMachineService
    {
        int RegisterNumber { get; set; }
        int ContentsOfOperationRegister { get; }
        string NameOperationReg { get; }
        string ErrorMessage { get; }
        string StringForPrinting { get; set; }
        int CheckType { get; set; }
        int OperationType { get; set; }
        double Quantity { get; set; }
        decimal Price { get; set; }
        int Tax1 { get; set; }
        int Tax2 { get; set; }
        int Tax3 { get; set; }
        int Tax4 { get; set; }
        decimal Summ1 { get; set; }
        string Connect();
        string OpenShift();
        string CloseShift();
        string ShowProperties();
        string PrintReportWithoutCleaning();
        string Sale();
        string CloseCheck();
        string CutCheck();
        string Disconnect();
        string ReturnSale();
        string GetOperationReg();
        string CancelReceipt();
        ECRModeEnum ECRMode();
    }
}
