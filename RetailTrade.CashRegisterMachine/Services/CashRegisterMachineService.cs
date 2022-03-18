using System;

namespace RetailTrade.CashRegisterMachine.Services
{
    public enum CashRegisterResult
    {
        /// <summary>
        /// Нет соединение
        /// </summary>
        NoConnection,

        /// <summary>
        /// Смена открыта
        /// </summary>
        SessionOpened

    }

    public enum ShtrihMResult
    {
        /// <summary>
        /// Принтер в рабочем режиме
        /// </summary>
        PrinterWorkingMode,

        /// <summary>
        /// Выдача данных
        /// </summary>
        DataOutput,

        /// <summary>
        /// Открытая смена, 24 часа не кончились
        /// </summary>
        OpenSession24HoursAreNotOver,

        /// <summary>
        /// Открытая смена, 24 часа кончились
        /// </summary>
        OpenSession24HoursAreOver,

        /// <summary>
        /// Закрытая смена
        /// </summary>
        ClosedSession,

        /// <summary>
        /// Блокировка по неправильному паролю налогового инспектора
        /// </summary>
        BlockingIncorrectPasswordInspector,

        /// <summary>
        /// Ожидание подтверждения ввода даты
        /// </summary>
        WaitingConfirmationDateEntry,

        /// <summary>
        /// Разрешение изменения положения десятичной точки
        /// </summary>
        PermissionChangePositionDecimalPoint,

        /// <summary>
        /// Открытый документ
        /// </summary>
        OpenDocument
    }

    public class CashRegisterMachineService : ICashRegisterMachineService
    {
        public event Action<CashRegisterResult> CashRegisterEvent;

        public void Close(CashRegisterType cashRegisterType)
        {
            throw new NotImplementedException();
        }

        public void Open(CashRegisterType cashRegisterType)
        {
            switch (cashRegisterType)
            {
                case CashRegisterType.OkaMF:
                    break;
                case CashRegisterType.ShtrihM:
                    OpenShtrihM();
                    break;
            }
        }

        public void SetAppSetting(string key, string value)
        {
            throw new NotImplementedException();
        }

        private void OpenShtrihM()
        {
            if (ShtrihM.CheckConnection() == 0)
            {
                ShtrihM.OpenSession();
                CashRegisterEvent?.Invoke(CashRegisterResult.SessionOpened);
            }
            else
            {
                CashRegisterEvent?.Invoke(CashRegisterResult.NoConnection);
            }
        }
    }
}
