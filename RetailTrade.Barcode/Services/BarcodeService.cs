using System.Configuration;
using System.IO.Ports;
using System.Reflection;

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
                    CloseComBarcode();
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
                    OpenComBarcode();
                    break;
                case BarcodeDevice.Zebra:
                    break;
                case BarcodeDevice.USB:
                    break;
                default:
                    break;
            }
        }

        public void SetAppSetting(string key, string value)
        {
            try
            {
                var asmPath = Assembly.GetExecutingAssembly().Location;
                var config = ConfigurationManager.OpenExeConfiguration(asmPath);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Full, true);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error reading configuration setting", e);
            }
        }

        #endregion

        #region Action

        public event Action<string> OnBarcodeEvent;

        #endregion

        #region Private Voids

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                Thread.Sleep(500);
                OnBarcodeEvent?.Invoke(Replace(_serialPort.ReadExisting()));
                _serialPort.DiscardInBuffer();
            }
        }

        private static string Replace(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return text.Replace("\r", string.Empty);
            }
            return string.Empty;
        }

        private static string GetAppSetting(string key)
        {
            try
            {
                var asmPath = Assembly.GetExecutingAssembly().Location;
                var config = ConfigurationManager.OpenExeConfiguration(asmPath);
                var setting = config.AppSettings.Settings[key];
                return setting.Value;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Ошибка при чтении настроек конфигурации ComBarcode.", e);
            }
        }

        #endregion

        #region ComBarcode

        private void OpenComBarcode()
        {
            string comPortName = GetAppSetting("ComPortName");
            int comPortSpeed = int.TryParse(GetAppSetting("ComPortSpeed"), out int speed) ? 9600 : speed;
            if (_serialPort == null)
            {
                if (!string.IsNullOrEmpty(comPortName))
                {
                    _serialPort = new()
                    {
                        PortName = comPortName,
                        BaudRate = comPortSpeed
                    };
                    _serialPort.DataReceived += SerialPort_DataReceived;
                    _serialPort.Open();
                }
            }
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
                _serialPort.PortName = comPortName;
                _serialPort.BaudRate = comPortSpeed;
                _serialPort.DataReceived += SerialPort_DataReceived;
                _serialPort.Open();
            }
        }

        private void CloseComBarcode()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Close();
                }
            }
        }

        public void ComBarcodeSettings(string comName, int speed)
        {
            
        }

        #endregion

        #region Zebra



        #endregion

        #region USB



        #endregion
    }
}
