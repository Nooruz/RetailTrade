using RetailTrade.Barcode;
using System;

namespace RetailTradeClient.State.Barcode
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
            ZebraScanner.OnScanningEvent += ZebraScanner_OnScanningEvent;
        }

        private void ZebraScanner_OnScanningEvent(string barcode)
        {
            OnBarcodeEvent?.Invoke(barcode);
        }
    }
}
