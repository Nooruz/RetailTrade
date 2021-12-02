using DrvFRLib;

namespace RetailTrade.CashRegisterMachine
{
    public static class ShtrihM
    {
        #region Private Members

        private static readonly DrvFR drvFR;

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

        #endregion

        #region Constructor

        static ShtrihM()
        {
            drvFR = new DrvFR();
        }

        #endregion

        #region Public Static Voids

        public static void Connect()
        {
            drvFR.Connect();
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

        #endregion
    }
}
