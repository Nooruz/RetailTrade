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
        void Open(BarcodeDevice barcodeDevice, string comPortName, int speed);
        void Close(BarcodeDevice barcodeDevice);
    }
}
