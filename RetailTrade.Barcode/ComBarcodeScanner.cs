using System.IO.Ports;

namespace RetailTrade.Barcode
{
    public static class ComBarcodeScanner
    {
        #region Private Static Members

        private static SerialPort _serialPort;

        #endregion

        #region Event Actions

        public static event Action<string> OnBarcodeEvent; 

        #endregion

        #region Constructor

        static ComBarcodeScanner()
        {
            _serialPort = new()
            {
                PortName = "COM4",
                BaudRate = 9600,
                ReadTimeout = 1000,
            };
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        #endregion

        #region Private Static Voids

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                Open();
            }
            Thread.Sleep(1000);
            OnBarcodeEvent?.Invoke(Replace(_serialPort.ReadExisting()));
            _serialPort.DiscardInBuffer();
        }

        private static string Replace(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return text.Replace("\r", string.Empty);
            }
            return string.Empty;
        }

        #endregion

        #region Public Static Voids

        public static void Open()
        {
            if (_serialPort != null && !_serialPort.IsOpen)
            {
                _serialPort.Open();
            }            
        }

        public static void Close()
        {
            _serialPort.Close();
        }

        #endregion
    }
}
