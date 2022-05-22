namespace RetailTrade.Barcode.Services
{
    public enum BarcodeDevice
    {
        Com,
        Zebra,
        USB,
        Honeywell
    }
    public interface IBarcodeService
    {
        void Open(BarcodeDevice barcodeDevice);
        void Close(BarcodeDevice barcodeDevice);
        event Action<string> OnBarcodeEvent;
    }
}
