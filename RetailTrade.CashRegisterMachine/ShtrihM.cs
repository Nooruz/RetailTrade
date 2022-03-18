using DrvFRLib;
using RetailTrade.CashRegisterMachine.Properties;
using System;

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

        public static int RegisterNumber
        {
            get => drvFR.RegisterNumber;
            set => drvFR.RegisterNumber = value;
        }
        public static int OperationType
        {
            get => drvFR.OperationType;
            set => drvFR.OperationType = value;
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
        public static int Password
        {
            get => drvFR.Password;
            set => drvFR.Password = value;
        }
        public static int Department
        {
            get => drvFR.Department;
            set => drvFR.Department = value;
        }
        public static int OperationNumber => drvFR.OperatorNumber;
        public static int OpenDocumentNumber => drvFR.OpenDocumentNumber;
        public static int ECRMode => drvFR.ECRMode;
        public static bool ReceiptRibbonIsPresent => drvFR.ReceiptRibbonIsPresent;
        public static string NameOperationReg => drvFR.NameOperationReg;
        public static int ReceiptNumber => drvFR.ReceiptNumber;
        public static string ResultCodeDescription => drvFR.ResultCodeDescription;

        /// <summary>
        /// Сменадагы количество чека
        /// </summary>
        public static int ContentsOfOperationRegister => drvFR.ContentsOfOperationRegister;

        #endregion

        #region Constructor

        static ShtrihM()
        {
            drvFR = new DrvFR()
            {
                ComNumber = Settings.Default.ComNumber,
                BaudRate = Settings.Default.BaudRate,
                Timeout = Settings.Default.Timeout
            };
        }

        #endregion

        #region Public Static Voids

        public static void Connect()
        {            
            drvFR.Connect();
        }

        public static int FNGetCurrentSessionParams()
        {
            return drvFR.FNGetCurrentSessionParams();
        }

        public static void ShowProperties()
        {
            drvFR.ShowProperties();

            Settings.Default.ComNumber = drvFR.ComNumber;
            Settings.Default.BaudRate = drvFR.BaudRate;
            Settings.Default.Timeout = drvFR.Timeout;
            Settings.Default.Save();

            drvFR.StringQuantity = 24;

            drvFR.SetExchangeParam();
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
        public static int OpenSession()
        {
            return drvFR.OpenSession();
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

        /// <summary>
        /// Закрыть чек
        /// </summary>
        public static void CloseCheck()
        {
            drvFR.CloseCheck();
        }

        public static int ReadLastReceipt()
        {
            return drvFR.ReadLastReceipt();
        }

        public static string GetOperationReg()
        {
            int checkNumber = drvFR.GetOperationReg();
            return checkNumber.ToString();
        }

        /// <summary>
        /// Закрытие чек с резултатом
        /// </summary>
        /// <returns></returns>
        public static object CloseCheckWithResult()
        {
            return drvFR.CloseCheckWithResult();
        }

        /// <summary>
        /// Отрезать чек
        /// </summary>
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
                3 => "Открытая смена, 24 часа кончились",
                4 => "Закрытая смена",
                5 => "Блокировка по неправильному паролю налогового инспектора",
                6 => "Ожидание подтверждения ввода даты",
                7 => "Разрешение изменения положения десятичной точки",
                8 => "Открытый документ",
                9 => "Режим разрешения технологического обнуления",
                10 => "Тестовый прогон",
                11 => "Печать полного фискального отчета",
                12 => "Печать длинного отчета ЭКЛЗ",
                13 => "Работа с фискальным подкладным документом",
                14 => "Печать подкладного документа",
                15 => "Фискальный подкладной документ сформирован",
                _ => "",
            };
        }

        /// <summary>
        /// Анулировать чек
        /// </summary>
        /// <returns></returns>
        public static int SysAdminCancelCheck()
        {
            return drvFR.SysAdminCancelCheck();            
        }

        /// <summary>
        /// Снять отчет с гашением
        /// </summary>
        public static void PrintReportWithCleaning()
        {
            drvFR.PrintReportWithCleaning();
        }

        /// <summary>
        /// Установить текущее время ККМ
        /// </summary>
        public static void SetTime()
        {
            if (ECRMode == 4 || ECRMode == 7 || ECRMode == 9)
            {
                drvFR.Time = DateTime.Now;
                drvFR.SetTime();
            }
        }

        public static int ReturnSale()
        {
            return drvFR.ReturnSale();
        }

        #endregion
    }
}
