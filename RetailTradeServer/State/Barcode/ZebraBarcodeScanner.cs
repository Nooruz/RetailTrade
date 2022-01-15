using RetailTrade.Barcode;
using System;

namespace RetailTradeServer.State.Barcode
{
    public class ZebraBarcodeScanner : IZebraBarcodeScanner
    {
        public event Action<string> OnBarcodeEvent;

        public void Close()
        {
            _ = ZebraScanner.Close();
        }

        public void Open()
        {
            _ = ZebraScanner.Open();
            ZebraScanner.OnScanningEvent += ((string barcode) => OnBarcodeEvent?.Invoke(barcode));
        }
    }
}
