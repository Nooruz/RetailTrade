using SMDrvFR1CLib;
using System;

namespace RetailTrade.CashRegisterMachine
{

    public static class OkaMf
    {
        private static SMDrvFR1CClass okaMF;
        static OkaMf()
        {
            okaMF = new SMDrvFR1CClass();
            
        }

        public static bool Open(out string result)
        {
            try
            {
                return okaMF.Open(out result);
            }
            catch (Exception e)
            {
                result = e.Message;
                return false;
            }
        }
    }
}
