using System;

namespace RetailTradeServer.State.Barcode
{
    public interface IComBarcodeService
    {
        void Open();
        void Close();
        event Action<string> OnBarcodeEvent;
    }
}
