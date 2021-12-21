using CoreScanner;

namespace RetailTrade.BarcodeScaner
{
    public static class BarcodeScaner
    {
        static CCoreScanner scanner;

        public static void Open()
        {
            scanner = new CCoreScannerClass();

            int status;

            short[] scanerTypes = new short[1];
            scanerTypes[0] = 1;

            scanner.Open(0, scanerTypes, 1, out status);

            short numberOfScanner;
            int[] connectedScannerIDList = new int[10];
            string outXML;

            scanner.GetScanners(out numberOfScanner, connectedScannerIDList, out outXML, out status);

            var sd = outXML;

            scanner.BarcodeEvent += CCoreScannerClass_BarcodeEvent;
        }

        private static void CCoreScannerClass_BarcodeEvent(short eventType, ref string pscanData)
        {
            
        }
    }
}