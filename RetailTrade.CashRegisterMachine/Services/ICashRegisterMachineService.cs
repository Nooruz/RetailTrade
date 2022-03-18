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
        void Open(CashRegisterType cashRegisterType);
        void Close(CashRegisterType cashRegisterType);
        void SetAppSetting(string key, string value);
        event Action<CashRegisterResult> CashRegisterEvent;
    }
}
