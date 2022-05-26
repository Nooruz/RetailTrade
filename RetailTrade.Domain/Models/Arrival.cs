using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Приход
    /// </summary>
    public class Arrival : DomainObject
    {
        #region Private Members

        private DateTime _arrivalDate;
        private string _invoiceNumber;
        private DateTime? _invoiceDate;
        private int _supplierId;
        private int? _wareHouseId;
        private decimal _sum;
        private string _comment;

        #endregion

        #region Public Properties

        public DateTime ArrivalDate
        {
            get => _arrivalDate;
            set
            {
                _arrivalDate = value;
                OnPropertyChanged(nameof(ArrivalDate));
            }
        }
        public string InvoiceNumber
        {
            get => _invoiceNumber;
            set
            {
                _invoiceNumber = value;
                OnPropertyChanged(nameof(InvoiceNumber));
            }
        }
        public DateTime? InvoiceDate
        {
            get => _invoiceDate;
            set
            {
                _invoiceDate = value;
                OnPropertyChanged(nameof(InvoiceDate));
            }
        }
        public int SupplierId
        {
            get => _supplierId;
            set
            {
                _supplierId = value;
                OnPropertyChanged(nameof(SupplierId));
            }
        }         
        public int? WareHouseId
        {
            get => _wareHouseId;
            set
            {
                _wareHouseId = value;
                OnPropertyChanged(nameof(WareHouseId));
            }
        }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                OnPropertyChanged(nameof(Sum));
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        public Supplier Supplier { get; set; }
        public WareHouse WareHouse { get; set; }
        public ICollection<ArrivalProduct> ArrivalProducts { get; set; }

        #endregion
    }
}
