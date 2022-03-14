using RetailTrade.Barcode;
using System;
using System.IO.Ports;
using System.Linq;
using System.Management;

namespace RetailTradeClient.State.Barcode
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

        public void SetParameters(string portName, int baudRate)
        {
            ComBarcodeScanner.SetParameters(portName, baudRate);
        }

        private string SearchBarcodeScanner()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());
                var portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();
                foreach (string s in portList)
                {
                    var port = s;
                }
            }
            return "";
        }
    }
}
