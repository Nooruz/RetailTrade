using System;
using RetailTrade.Barcode;

namespace RetailTradeServer.State.Barcode
{
    public class ComBarcodeService : IComBarcodeService
    {
        public event Action<string> OnBarcodeEvent;

        public void Close()
        {
            ComBarcodeScanner.Close();
        }

        public void Open()
        {
            ComBarcodeScanner.Open();
            ComBarcodeScanner.OnBarcodeEvent += ((string barcode) => OnBarcodeEvent?.Invoke(barcode));
        }
    }
}
