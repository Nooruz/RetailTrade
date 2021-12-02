using RetailTradeClient.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class CommunicationSettingsViewModel : BaseDialogViewModel
    {
        #region Private Members

        //private string _selectedCOMPort = SerialPort.GetPortNames().Length == 0 ? string.Empty : SerialPort.GetPortNames()[0];
        private int _selectedBaudRate = 115200;
        private int _timeoutBetweenComputer = 500;
        private int _timeout = 7500;
        //private SerialPort Port;

        #endregion

        #region Public Properties

        /// <summary>
        /// Список доступных COM портов
        /// </summary>
        //public List<string> COMPorts => new(SerialPort.GetPortNames());

        public ObservableCollection<CRM> GetCRMs { get; set; }

        /// <summary>
        /// Скорость бодрейт
        /// </summary>
        public List<int> BaudRates => new(new int[] { 2400, 4800, 9600, 19200, 38400, 57600, 115200 });

        /// <summary>
        /// Выбранный COM порт
        /// </summary>
        //public string SelectedCOMPort
        //{
        //    get => _selectedCOMPort;
        //    set
        //    {
        //        _selectedCOMPort = value;
        //        OnPropertyChanged(nameof(SelectedCOMPort));
        //    }
        //}

        /// <summary>
        /// Выбранный бодрейт
        /// </summary>
        public int SelectedBaudRate
        {
            get => _selectedBaudRate;
            set
            {
                _selectedBaudRate = value;
                OnPropertyChanged(nameof(SelectedBaudRate));
            }
        }


        /// <summary>
        /// Тайм-аут между ком. (мс)
        /// </summary>
        public int TimeoutBetweenComputer
        {
            get => _timeoutBetweenComputer;
            set
            {
                _timeoutBetweenComputer = value;
                OnPropertyChanged(nameof(TimeoutBetweenComputer));
            }
        }

        /// <summary>
        /// Тайм-аут (мс)
        /// </summary>
        public int Timeout
        {
            get => _timeout;
            set
            {
                _timeout = value;
                OnPropertyChanged(nameof(Timeout));
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Поиск ККМ
        /// </summary>
        public ICommand SearchCRMCommand { get; }

        /// <summary>
        /// Получить номер ККМ
        /// </summary>
        public ICommand GetCRMNumberCommand { get; }

        #endregion

        #region Constructor

        public CommunicationSettingsViewModel()
        {
            SearchCRMCommand = new RelayCommand(SearchCRM);
            GetCRMNumberCommand = new RelayCommand(GetCRMNumber);
            GetCRMs = new ObservableCollection<CRM>();
        }

        #endregion

        #region Private Voids

        private void GetCRMNumber()
        {

        }

        private void SearchCRM()
        {
            //object err;
            //GetStateAC(out err);
            //ShowMessageTest();

            //ShowMessageTest1("");

            var r = ShowFunctionMessage("COM7");
        }

        [DllImport("drvasOkaMF_KZ.dll")]
        private static extern byte GetStateAC(string namePort, uint speedPort, uint timeOut, uint numKKM);

        [DllImport("Test.dll")]
        private static extern string ShowFunctionMessage(string message);

        #endregion
    }

    public class CRM
    {
        public string COMPort { get; set; }
        public int CRMNumber { get; set; }
        public int BaudRate { get; set; }
    }
}
