using System;

namespace RetailTradeClient.State.Barcode
{
    public interface IZebraBarcodeScanner
    {
        void Open();
        void Close();
        event Action<string> OnBarcodeEvent;
    }
}
