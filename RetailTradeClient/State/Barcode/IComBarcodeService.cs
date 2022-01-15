using System;

namespace RetailTradeClient.State.Barcode
{
    public interface IComBarcodeService
    {
        void Open();
        void Close();
        event Action<string> OnBarcodeEvent;
    }
}
