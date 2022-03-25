using System.IO.Ports;
using System.Text;
using System.Xml.Linq;

namespace RetailTrade.Barcode.Services
{
    public enum Opcode
    {
        /// <summary>
        /// Gets the version of CoreScanner
        /// </summary>
        GetVersion = 1000,

        /// <summary>
        /// Register for API events
        /// </summary>
        RegisterForEvents = 1001,

        /// <summary>
        /// Unregister for API events
        /// </summary>
        UnregisterForEvents = 1002,

        /// <summary>
        /// Get Bluetooth scanner pairing bar code
        /// </summary>
        GetPairingBarcode = 1005,

        /// <summary>
        /// Claim a specific device
        /// </summary>
        ClaimDevice = 1500,

        /// <summary>
        /// Release a specific device
        /// </summary>
        ReleaseDevice = 1501,

        /// <summary>
        /// Abort MacroPDF of a specified scanner
        /// </summary>
        AbortMacroPdf = 2000,

        /// <summary>
        /// Abort firmware update process of a specified scanner, while in progress
        /// </summary>
        AbortUpdateFirmware = 2001,

        /// <summary>
        /// Turn Aim off
        /// </summary>
        DeviceAimOff = 2002,

        /// <summary>
        /// Turn Aim on
        /// </summary>
        DeviceAimOn = 2003,

        /// <summary>
        /// Flush MacroPDF of a specified scanner
        /// </summary>
        FlushMacroPdf = 2005,

        /// <summary>
        /// Pull the trigger of a specified scanner
        /// </summary>
        DevicePullTrigger = 2011,

        /// <summary>
        /// Release the trigger of a specified scanner
        /// </summary>
        DeviceReleaseTrigger = 2012,

        /// <summary>
        /// Disable scanning on a specified scanner
        /// </summary>
        DeviceScanDisable = 2013,

        /// <summary>
        /// Enable scanning on a specified scanner
        /// </summary>
        DeviceScanEnable = 2014,

        /// <summary>
        /// Set parameters to default values of a specified scanner
        /// </summary>
        SetParameterDefaults = 2015,

        /// <summary>
        /// Set parameters of a specified scanner
        /// </summary>
        DeviceSetParameters = 2016,

        /// <summary>
        /// Set and persist parameters of a specified scanner
        /// </summary>
        SetParameterPersistance = 2017,

        /// <summary>
        /// Reboot a specified scanner
        /// </summary>
        RebootScanner = 2019,

        /// <summary>
        /// Disconnect the specified Bluetooth scanner
        /// </summary>
        DisconnectBluetoothScanner = 2023,

        /// <summary>
        /// Change a specified scanner to snapshot mode 
        /// </summary>
        DeviceCaptureImage = 3000,

        /// <summary>
        /// Change a specified scanner to decode mode 
        /// </summary>
        DeviceCaptureBarcode = 3500,

        /// <summary>
        /// Change a specified scanner to video mode 
        /// </summary>
        DeviceCaptureVideo = 4000,

        /// <summary>
        /// Get all the attributes of a specified scanner
        /// </summary>
        RsmAttrGetAll = 5000,

        /// <summary>
        /// Get the attribute values(s) of specified scanner
        /// </summary>
        RsmAttrGet = 5001,

        /// <summary>
        /// Get the next attribute to a given attribute of specified scanner
        /// </summary>
        RsmAttrGetNext = 5002,

        /// <summary>
        /// Set the attribute values(s) of specified scanner
        /// </summary>
        RsmAttrSet = 5004,

        /// <summary>
        /// Store and persist the attribute values(s) of specified scanner
        /// </summary>
        RsmAttrStore = 5005,

        /// <summary>
        /// Get the topology of the connected devices
        /// </summary>
        GetDeviceTopology = 5006,

        /// <summary>
        /// Remove all Symbol device entries from registry
        /// </summary>
        UninstallSymbolDevices = 5010,

        /// <summary>
        /// Start (flashing) the updated firmware
        /// </summary>
        StartNewFirmware = 5014,

        /// <summary>
        /// Update the firmware to a specified scanner
        /// </summary>
        UpdateFirmware = 5016,

        /// <summary>
        /// Update the firmware to a specified scanner using a scanner plug-in
        /// </summary>
        UpdateFirmwareFromPlugin = 5017,

        /// <summary>
        /// Update good scan tone of the scanner with specified wav file
        /// </summary>
        UpdateDecodeTone = 5050,

        /// <summary>
        /// Erase good scan tone of the scanner
        /// </summary>
        EraseDecodeTone = 5051,

        /// <summary>
        /// Perform an action involving scanner beeper/LEDs
        /// </summary>
        SetAction = 6000,

        /// <summary>
        /// Set the serial port settings of a NIXDORF Mode-B scanner
        /// </summary>
        DeviceSetSerialPortSettings = 6101,

        /// <summary>
        /// Switch the USB host mode of a specified scanner
        /// </summary>
        DeviceSwitchHostMode = 6200,

        /// <summary>
        /// Switch CDC devices
        /// </summary>
        SwitchCdcDevices = 6201,

        /// <summary>
        /// Enable/Disable keyboard emulation mode
        /// </summary>
        KeyboardEmulatorEnable = 6300,

        /// <summary>
        /// Set the locale for keyboard emulation mode
        /// </summary>
        KeyboardEmulatorSetLocale = 6301,

        /// <summary>
        /// Get current configuration of the HID keyboard emulator
        /// </summary>
        KeyboardEmulatorGetConfig = 6302,

        /// <summary>
        ///  Configure Driver ADF
        /// </summary>
        ConfigureDADF = 6400,

        /// <summary>
        /// Reset Driver ADF
        /// </summary>
        ResetDADF = 6401,

        /// <summary>
        /// Measure the weight on the scanner's platter and get the value
        /// </summary>
        ScaleReadWeight = 7000,

        /// <summary>
        ///  Zero the scale
        /// </summary>
        ScaleZeroScale = 7002,

        /// <summary>
        /// Reset the scale
        /// </summary>
        ScaleSystemReset = 7015,
    }

    public enum Status
    {
        /// <summary>
        /// Status success
        /// </summary>
        Success = 0,

        /// <summary>
        /// Status locked
        /// </summary>
        Locked = 10
    }

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
                    CloseZebra();
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
                    OpenZebra();
                    break;
                case BarcodeDevice.USB:
                    break;
                default:
                    break;
            }
        }

        public void Open(BarcodeDevice barcodeDevice, string comPortName, int speed)
        {
            if (barcodeDevice == BarcodeDevice.Com)
            {
                OpenComBarcode(comPortName, speed);
            }
        }

        #endregion

        #region Action

        public event Action<string> OnBarcodeEvent;

        #endregion

        #region Private Voids

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
                else
                {
                    Thread.Sleep(100);
                    OnBarcodeEvent?.Invoke(Replace(_serialPort.ReadExisting()));
                    _serialPort.DiscardInBuffer();
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private static string Replace(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    return text.Replace("\r", string.Empty);
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return string.Empty;
        }

        #endregion

        #region ComBarcode

        private void OpenComBarcode(string comPortName, int speed)
        {
            try
            {
                if (!string.IsNullOrEmpty(comPortName))
                {
                    if (_serialPort != null)
                    {
                        if (!_serialPort.IsOpen)
                        {
                            _serialPort.Open();
                            _serialPort.DiscardInBuffer();
                            _serialPort.DataReceived += SerialPort_DataReceived;
                        }
                    }
                    else
                    {
                        _serialPort = new()
                        {
                            PortName = comPortName,
                            BaudRate = speed,
                            ReadTimeout = 1000
                        };
                        _serialPort.Open();
                        _serialPort.DiscardInBuffer();
                        _serialPort.DataReceived += SerialPort_DataReceived;
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void CloseComBarcode()
        {
            try
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
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Zebra

        private void OpenZebra()
        {
            //try
            //{
            //    if (_scanner == null)
            //    {
            //        _scanner = new();
            //    }

            //    int appHandle = 0;
            //    short[] scannerTypes = new short[1];
            //    scannerTypes[0] = 1;
            //    short numberOfScannerTypes = 1;

            //    _scanner.Open(appHandle, scannerTypes, numberOfScannerTypes, out int status);

            //    string inXML = "<inArgs>" +
            //                       "<cmdArgs>" +
            //                           "<arg-int>6</arg-int>" + // Number of events you want to subscribe
            //                           "<arg-int>1,2,4,8,16,32</arg-int>" + // Comma separated event IDs
            //                       "</cmdArgs>" +
            //                   "</inArgs>";

            //    _scanner.ExecCommand((int)Opcode.RegisterForEvents, ref inXML, out string outXML, out status);

            //    if (status == (int)Status.Success)
            //    {
            //        _scanner.BarcodeEvent += new _ICoreScannerEvents_BarcodeEventEventHandler(OnZebraBarcodeEvent);
            //    }
            //}
            //catch (Exception)
            //{
            //    //ignore
            //}
        }
        private void OnZebraBarcodeEvent(short eventType, ref string pscanData)
        {
            OnBarcodeEvent?.Invoke(Encoding.ASCII.GetString(FromHex(XElement.Parse(pscanData).Descendants("datalabel").Single().Value.Replace(" ", string.Empty))));
        }
        private static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 4];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 4, 4), 16);
            }
            return raw;
        }
        private void CloseZebra()
        {
            //_scanner.Close(0, out _);
        }

        #endregion

        #region USB



        #endregion
    }
}
