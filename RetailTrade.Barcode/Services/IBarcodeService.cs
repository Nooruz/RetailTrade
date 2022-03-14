namespace RetailTrade.Barcode.Services
{
    public enum BarcodeDevice
    {
        Com,
        Zebra,
        USB
    }
    public interface IBarcodeService
    {
        void Open(BarcodeDevice barcodeDevice);
        void Close(BarcodeDevice barcodeDevice);
        void SetAppSetting(string key, string value);
        event Action<string> OnBarcodeEvent;
    }
}
