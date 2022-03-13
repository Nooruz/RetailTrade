using System.IO.Ports;

namespace RetailTrade.Barcode.Services
{
    public class BarcodeService : IBarcodeService
    {
        #region Private Members

        private SerialPort _serialPort;

        #endregion

        #region Constructor

        public BarcodeService()
        {

        }

        #endregion

        #region Public Voids

        public void Close(BarcodeDevice barcodeDevice)
        {
            switch (barcodeDevice)
            {
                case BarcodeDevice.Com:
                    break;
                case BarcodeDevice.Zebra:
                    break;
                case BarcodeDevice.USB:
                    break;
                default:
                    break;
            }
        }

        public void Open(BarcodeDevice barcodeDevice)
        {
            switch (barcodeDevice)
            {
                case BarcodeDevice.Com:
                    break;
                case BarcodeDevice.Zebra:
                    break;
                case BarcodeDevice.USB:
                    break;
                default:
                    break;
            }
        }

        public void Open(BarcodeDevice barcodeDevice, string comPortName, int speed)
        {
            
        }

        #endregion

        #region Private Voids

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
        }

        #endregion

        #region ComBarcode

        private void OpenComBarcode()
        {
            if (!string.IsNullOrEmpty(Properties.Resources.BarcodeComName.ToString()) && speed > 0)
            {
                if (_serialPort == null)
                {
                    _serialPort = new()
                    {
                        PortName = comPortName,
                        BaudRate = speed,
                        ReadTimeout = 1000,
                    };
                }
                else
                {
                    _serialPort.PortName = comPortName;
                }
                _serialPort.DataReceived += SerialPort_DataReceived;
            }
        }

        #endregion

        #region Zebra



        #endregion

        #region USB



        #endregion
    }
}
