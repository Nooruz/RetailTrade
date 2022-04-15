using System;

namespace RetailTrade.CashRegisterMachine.Services
{

    public class CashRegisterMachineService : ICashRegisterMachineService
    {
        #region Public Properties

        public string ErrorMessage { get; private set; }
        public string StringForPrinting
        {
            get => ShtrihM.StringForPrinting;
            set => ShtrihM.StringForPrinting = value;
        }
        public int RegisterNumber
        {
            get => ShtrihM.RegisterNumber;
            set => ShtrihM.RegisterNumber = value;
        }
        public int ContentsOfOperationRegister
        {
            get => ShtrihM.ContentsOfOperationRegister;
        }
        public string NameOperationReg
        {
            get => ShtrihM.NameOperationReg;
        }
        public int CheckType
        {
            get => ShtrihM.CheckType;
            set => ShtrihM.CheckType = value;
        }
        public int OperationType
        {
            get => ShtrihM.OperationType;
            set => ShtrihM.OperationType = value;
        }
        public double Quantity
        {
            get => ShtrihM.Quantity;
            set => ShtrihM.Quantity = value;
        }
        public decimal Price
        {
            get => ShtrihM.Price;
            set => ShtrihM.Price = value;
        }
        public int Tax1
        {
            get => ShtrihM.Tax1;
            set => ShtrihM.Tax1 = value;
        }
        public int Tax2
        {
            get => ShtrihM.Tax2;
            set => ShtrihM.Tax2 = value;
        }
        public int Tax3
        {
            get => ShtrihM.Tax3;
            set => ShtrihM.Tax3 = value;
        }
        public int Tax4
        {
            get => ShtrihM.Tax4;
            set => ShtrihM.Tax4 = value;
        }
        public decimal Summ1
        {
            get => ShtrihM.Summ1;
            set => ShtrihM.Summ1 = value;
        }

        #endregion

        #region Public Voids

        public string CloseShift()
        {
            try
            {
                return Result(ShtrihM.PrintReportWithCleaning());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string Connect()
        {
            try
            {
                return Result(ShtrihM.Connect());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string OpenShift()
        {
            try
            {
                return Result(ShtrihM.OpenSession());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string ShowProperties()
        {
            try
            {
                return Result(ShtrihM.ShowProperties());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string PrintReportWithoutCleaning()
        {
            try
            {
                return Result(ShtrihM.PrintReportWithoutCleaning());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string Sale()
        {
            try
            {
                return Result(ShtrihM.Sale());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string CloseCheck()
        {
            try
            {
                return Result(ShtrihM.CloseCheck());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string CutCheck()
        {
            try
            {
                return Result(ShtrihM.CutCheck());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string Disconnect()
        {
            try
            {
                return Result(ShtrihM.Disconnect());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string ReturnSale()
        {
            try
            {
                return Result(ShtrihM.ReturnSale());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string GetOperationReg()
        {
            try
            {
                return Result(ShtrihM.GetOperationReg());
            }
            catch (Exception)
            {
                //ignore
            }
            return string.Empty;
        }
        public ECRModeEnum ECRMode()
        {
            return ShtrihM.GetECRMode();
        }

        #endregion

        #region Private Voids

        private string Result(int errorCode)
        {
            switch (errorCode)
            {
                case -19:
                    ErrorMessage = "Ключ защиты не найден или не введена лицензия или лицензия не действительна";
                    return ErrorMessage;
                case -18:
                    ErrorMessage = "Порт блокирован";
                    return ErrorMessage;
                case -17:
                    ErrorMessage = "Порт не открыт";
                    return ErrorMessage;
                case -16:
                    ErrorMessage = "Не удалось подключиться к серверу";
                    return ErrorMessage;
                case -15:
                    ErrorMessage = "Невозможно изменение скорости при работе через КУ ТРК";
                    return ErrorMessage;
                case -14:
                    ErrorMessage = "Удаление активного логического устройства невозможно";
                    return ErrorMessage;
                case -13:
                    ErrorMessage = "Подытог чека не изменился";
                    return ErrorMessage;
                case -12:
                    ErrorMessage = "Не поддерживается в данной версии драйвера";
                    return ErrorMessage;
                case -11:
                    ErrorMessage = "Ошибка протокола";
                    return ErrorMessage;
                case -10:
                    ErrorMessage = "Неверный номер логического устройства";
                    return ErrorMessage;
                case -9:
                    ErrorMessage = "Параметр вне диапазона";
                    return ErrorMessage;
                case -8:
                    ErrorMessage = "Неопознанная ошибка";
                    return ErrorMessage;
                case -7:
                    ErrorMessage = "Неверная длина ответа";
                    return ErrorMessage;
                case -6:
                    ErrorMessage = "Нет связи";
                    return ErrorMessage;
                case -5:
                    ErrorMessage = "Нет связи";
                    return ErrorMessage;
                case -4:
                    ErrorMessage = "Нет связи";
                    return ErrorMessage;
                case -3:
                    ErrorMessage = "Сом-порт занят другим приложением";
                    return ErrorMessage;
                case -2:
                    ErrorMessage = "Сом-порт не доступен";
                    return ErrorMessage;
                case -1:
                    ErrorMessage = "Нет связи";
                    return ErrorMessage;
                case 0:
                    ErrorMessage = string.Empty;
                    return ErrorMessage;
                case 1:
                    ErrorMessage = "Неисправен накопитель ФП 1, ФП 2 или часы";
                    return ErrorMessage;
                case 2:
                    ErrorMessage = "Отсутствует ФП 1";
                    return ErrorMessage;
                case 3:
                    ErrorMessage = "Отсутствует ФП 2";
                    return ErrorMessage;
                case 4:
                    ErrorMessage = "Некорректные параметры в команде обращения к ФП";
                    return ErrorMessage;
                case 5:
                    ErrorMessage = "Нет запрошенных данных";
                    return ErrorMessage;
                case 6:
                    ErrorMessage = "ФП в режиме вывода данных";
                    return ErrorMessage;
                case 7:
                    ErrorMessage = "Некорректные параметры в команде для данной реализации ФП";
                    return ErrorMessage;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
