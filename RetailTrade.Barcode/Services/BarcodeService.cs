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



        #endregion

        #region ComBarcode

        private void OpenComBarcode(string comPortName, int speed)
        {
            if (_serialPort == null)
            {
                _serialPort = new()
                {
                    
                };
            }
        }

        #endregion

        #region Zebra



        #endregion

        #region USB



        #endregion
    }
}
