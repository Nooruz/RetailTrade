﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetailTrade.BarcodeScanner
{
    public static class ComBarcodeScanner
    {
        #region Private Static Members

        private static readonly SerialPort _serialPort;

        #endregion

        #region Event Actions

        public static event Action<string> OnBarcodeEvent;

        #endregion

        #region Constructor

        static ComBarcodeScanner()
        {
            _serialPort = new SerialPort()
            {
                PortName = "COM17",
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

        public static void Close()
        {
            try
            {
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