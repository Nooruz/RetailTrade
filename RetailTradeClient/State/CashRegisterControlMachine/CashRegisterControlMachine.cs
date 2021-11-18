using DrvFRLib;

namespace RetailTradeClient.State.CashRegisterControlMachine
{
    public class CashRegisterControlMachine : ICashRegisterControlMachine
    {
        #region Private Members

        private readonly DrvFR _cashRegisterControlMachine;

        #endregion

        #region Constructor

        public CashRegisterControlMachine(DrvFR cashRegisterControlMachine)
        {
            cashRegisterControlMachine.ComNumber = 7;
            cashRegisterControlMachine.BaudRate = 6;
            cashRegisterControlMachine.Timeout = 2100;

            cashRegisterControlMachine.SetExchangeParam();
            cashRegisterControlMachine.CheckConnection();

            _cashRegisterControlMachine = cashRegisterControlMachine;
        }

        #endregion

        public DrvFR GetCashRegisterControlMachine()
        {
            return  _cashRegisterControlMachine;
        }
    }
}
