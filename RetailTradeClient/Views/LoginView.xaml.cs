using RetailTrade.CashRegisterMachine;
using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace RetailTradeClient.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            Combo.Items.Clear();
            for (int i = 0; i < ports.Length; i++)
            {
                Combo.Items.Add(ports[i]);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //SerialPort serialPort = new(Combo.SelectedItem.ToString(), 115200, Parity.None, 8, StopBits.Two)
                //{
                //    Parity = Parity.None,
                //    ReadTimeout = 1000,
                //    WriteTimeout = 1000
                //};

                //serialPort.DataReceived += SerialPort_DataReceived;

                //serialPort.Open();

                string error;
                OkaMf.GetDescriptionError(out error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = (SerialPort)sender;
            try
            {

            }
            catch (Exception)
            {
                int buferSize = port.BytesToRead;
                MessageBox.Show(buferSize.ToString());
                for (int i = 0; i < buferSize; i++)
                {
                    byte bt = (byte)port.ReadByte();
                    MessageBox.Show(bt.ToString());
                }
            }
        }
    }
}
