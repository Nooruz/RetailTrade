using System;

namespace RetailTradeServer.State.Barcode
{
    public interface IComBarcodeService
    {
        void Open();
        void SetParameters(string portName, int baudRate);
        void Close();
        event Action<string> OnBarcodeEvent;
    }
}
