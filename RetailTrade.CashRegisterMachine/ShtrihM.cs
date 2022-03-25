using DrvFRLib;
using RetailTrade.CashRegisterMachine.Properties;
using System;
using System.Reflection;

namespace RetailTrade.CashRegisterMachine
{
    [AttributeUsage(AttributeTargets.All)]
    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            StringValue = value;
        }

        #endregion

    }

    public enum ECRModeEnum : int
    {
        [StringValue("Принтер в рабочем режиме.")]
        Mode0 = 0,

        [StringValue("Выдача данных.")]
        Mode1 = 1,

        [StringValue("Открытая смена, 24 часа не кончились.")]
        Mode2 = 2,

        [StringValue("Открытая смена, 24 часа кончились.")]
        Mode3 = 3,

        [StringValue("Закрытая смена.")]
        Mode4 = 4,

        [StringValue("Блокировка по неправильному паролю налогового инспектора.")]
        Mode5 = 5,

        [StringValue("Ожидание подтверждения ввода даты.")]
        Mode6 = 6,

        [StringValue("Разрешение изменения положения десятичной точки.")]
        Mode7 = 7,

        [StringValue("Открытый документ.")]
        Mode8 = 8,

        [StringValue("Режим разрешения технологического обнуления.")]
        Mode9 = 9,

        [StringValue("Тестовый прогон.")]
        Mode10 = 10,

        [StringValue("Печать полного фискального отчета.")]
        Mode11 = 11,

        [StringValue("Печать длинного отчета ЭКЛЗ.")]
        Mode12 = 12,

        [StringValue("Работа с фискальным подкладным документом.")]
        Mode13 = 13,

        [StringValue("Печать подкладного документа.")]
        Mode14 = 14,

        [StringValue("Фискальный подкладной документ сформирован.")]
        Mode15 = 15
    }

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

            _ = drvFR.ResultCode;
        }

        #endregion

        #region Public Static Voids

        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static ECRModeEnum GetECRMode()
        {
            return (ECRModeEnum)Enum.GetValues<ECRModeEnum>().GetValue(ECRMode);
        }

        public static int Connect()
        {            
            return drvFR.Connect();
        }

        public static int FNGetCurrentSessionParams()
        {
            return drvFR.FNGetCurrentSessionParams();
        }

        public static int ShowProperties()
        {
            if (drvFR.ShowProperties() == 0)
            {
                Settings.Default.ComNumber = drvFR.ComNumber;
                Settings.Default.BaudRate = drvFR.BaudRate;
                Settings.Default.Timeout = drvFR.Timeout;
                Settings.Default.Save();

                drvFR.StringQuantity = 24;

                return drvFR.SetExchangeParam();
            }
            else
            {
                return drvFR.ShowProperties();
            }
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
        public static int PrintReportWithoutCleaning()
        {
            return drvFR.PrintReportWithoutCleaning();
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

        public static int Sale()
        {
            return drvFR.Sale();
        }

        /// <summary>
        /// Закрыть чек
        /// </summary>
        public static int CloseCheck()
        {
            return drvFR.CloseCheck();
        }

        public static int ReadLastReceipt()
        {
            return drvFR.ReadLastReceipt();
        }

        public static int GetOperationReg()
        {
            return drvFR.GetOperationReg();
        }

        /// <summary>
        /// Закрытие чек с резултатом
        /// </summary>
        /// <returns></returns>
        public static int CloseCheckWithResult()
        {
            return drvFR.CloseCheckWithResult();
        }

        /// <summary>
        /// Отрезать чек
        /// </summary>
        public static int CutCheck()
        {
            return drvFR.CutCheck();
        }

        public static int Disconnect()
        {
            return drvFR.Disconnect();
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
        public static int PrintReportWithCleaning()
        {
            return drvFR.PrintReportWithCleaning();
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
