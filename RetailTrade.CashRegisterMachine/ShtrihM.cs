using DrvFRLib;
using RetailTrade.CashRegisterMachine.Properties;

namespace RetailTrade.CashRegisterMachine
{
    public static class ShtrihM
    {
        #region Private Members

        private static DrvFR drvFR;

        #endregion

        #region Public Static Properties

        public static int CheckType
        {
            get => drvFR.CheckType;
            set => drvFR.CheckType = value;
        }
        public static double Quantity
        {
            get => drvFR.Quantity;
            set => drvFR.Quantity = value;
        }
        public static decimal Price
        {
            get => drvFR.Price;
            set => drvFR.Price = value;
        }
        public static string StringForPrinting
        {
            get => drvFR.StringForPrinting;
            set => drvFR.StringForPrinting = value;
        }
        public static string BarCode
        {
            get => drvFR.BarCode;
            set => drvFR.BarCode = value;
        }
        public static int Tax1
        {
            get => drvFR.Tax1;
            set => drvFR.Tax1 = value;
        }
        public static int Tax2
        {
            get => drvFR.Tax2;
            set => drvFR.Tax2 = value;
        }
        public static int Tax3
        {
            get => drvFR.Tax3;
            set => drvFR.Tax3 = value;
        }
        public static int Tax4
        {
            get => drvFR.Tax4;
            set => drvFR.Tax4 = value;
        }
        public static decimal Summ1
        {
            get => drvFR.Summ1;
            set => drvFR.Summ1 = value;
        }

        public static int ECRMode
        {
            get => drvFR.ECRMode;
        }

        #endregion

        #region Constructor

        static ShtrihM()
        {
            drvFR = new DrvFR
            {
                ComNumber = 3,
                BaudRate = 6,
                Timeout = 2100
            };

            drvFR.SetExchangeParam();
            drvFR.CheckConnection();
        }

        #endregion

        #region Public Static Voids

        public static void Connect()
        {
            drvFR.Connect();
        }

        public static void ShowProperties()
        {
            drvFR.ShowProperties();

            Settings.Default.ComNumber = drvFR.ComNumber;
            Settings.Default.BaudRate = drvFR.BaudRate;
            Settings.Default.Timeout = drvFR.Timeout;
        }

        /// <summary>
        /// Краткий запрос
        /// </summary>
        public static void GetShortECRStatus()
        {
            drvFR.GetShortECRStatus();
        }

        /// <summary>
        /// Снаять отчет без гашения
        /// </summary>
        public static void PrintReportWithoutCleaning()
        {
            drvFR.PrintReportWithoutCleaning();
        }

        /// <summary>
        /// Открытие смены ККМ
        /// </summary>
        public static void OpenSession()
        {
            drvFR.OpenSession();
        }

        /// <summary>
        /// Проверка связи
        /// </summary>
        public static int CheckConnection()
        {
            return drvFR.CheckConnection();
        }

        public static void Sale()
        {
            drvFR.Sale();
        }

        public static void CloseCheck()
        {
            drvFR.CloseCheck();
        }

        public static void CutCheck()
        {
            drvFR.CutCheck();
        }

        public static void Disconnect()
        {
            drvFR.Disconnect();
        }

        public static string GetECRMode()
        {
            return drvFR.ECRMode switch
            {
                0 => "Принтер в рабочем режиме",
                1 => "Выдача данных",
                2 => "Открытая смена, 24 часа не кончились",
                3 => "",
                4 => "",
                5 => "",
                6 => "",
                7 => "",
                8 => "",
                9 => "",
                10 => "",
                11 => "",
                12 => "",
                13 => "",
                14 => "",
                15 => "",
                _ => "",
            };
        }

        #endregion
    }
}
