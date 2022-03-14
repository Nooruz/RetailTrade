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
        void ComBarcodeSettings(string comName, int speed);
        void SetAppSetting(string key, string value);
        event Action<string> OnBarcodeEvent;
    }
}
