using System;

namespace RetailTrade.Domain.Exceptions
{
    public class ShiftException : Exception
    {
        public DateTime OpeningShiftDate { get; set; }
        public DateTime ClosingShiftDate { get; set; }

        public ShiftException(DateTime openingShiftDate, DateTime closingDate)
        {
            OpeningShiftDate = openingShiftDate;
            ClosingShiftDate = closingDate;
        }

        public ShiftException(DateTime openingShiftDate, DateTime closingDate, string message) : base(message) 
        {
            OpeningShiftDate = openingShiftDate;
            ClosingShiftDate = closingDate;
        }

        public ShiftException(DateTime openingShiftDate, DateTime closingDate, string message, Exception innerException) : base(message, innerException)
        {
            OpeningShiftDate = openingShiftDate;
            ClosingShiftDate = closingDate;
        }
    }
}
