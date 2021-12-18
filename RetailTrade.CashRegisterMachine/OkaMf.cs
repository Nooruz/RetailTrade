using System.Runtime.InteropServices;

namespace RetailTrade.CashRegisterMachine
{
    
    public static class OkaMf
    {
        [DllImport("drvasOkaMF_KZ.dll")]
        public static extern void GetStateAC(
            [MarshalAs(UnmanagedType.BStr)]
            string NamePort,
            int SpeedPort,
            int TimeOut,
            int NumKKM,
            [MarshalAs(UnmanagedType.U1)]
            out byte ModeAC);

        [DllImport(@"drvasOkaMF_KZ.dll", EntryPoint= "GetDescriptionError")]
        public static extern void GetDescriptionError(out string DescriptionError);

        [DllImport(@"drvasOkaMF_KZ.dll", EntryPoint="?")]
        public static extern void FormShow();

        static OkaMf()
        {
            
        }
    }
}
