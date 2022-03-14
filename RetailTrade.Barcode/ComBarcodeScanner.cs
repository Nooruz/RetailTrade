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

        #region Private Static Voids

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                Open();
            }
            Thread.Sleep(500);
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
                try
                {
                    _serialPort.Open();
                }
                catch (Exception)
                {
                    //ignore
                }                
            }            
        }

        public static void SetParameters(string portName, int baudRate)
        {
            if (!string.IsNullOrEmpty(portName) && baudRate > 0)
            {
                try
                {
                    if (_serialPort == null)
                    {
                        _serialPort = new()
                        {
                            PortName = portName,
                            BaudRate = baudRate,
                            ReadTimeout = 1000,
                        };
                    }
                    _serialPort.DataReceived += SerialPort_DataReceived;
                }
                catch (Exception)
                {
                    //ignore
                }
            }
        }

        public static void Close()
        {
            try
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.Close();
            }
            catch (Exception)
            {
                //ignore
            }            
        }

        #endregion
    }
}
