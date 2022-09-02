using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Document : DomainObject
    {
        #region Private Members

        private int _documentTypeId;
        private DateTime _createdDate;
        private int _userId;
        private decimal _amount;
        private int? _wareHouseId;
        private string _comment;

        #endregion

        #region Public Properties

        public int DocumentTypeId
        {
            get => _documentTypeId;
            set
            {
                _documentTypeId = value;
                OnPropertyChanged(nameof(DocumentTypeId));
            }
        }
        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged(nameof(CreatedDate));
            }
        }
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
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
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        public DocumentType DocumentType { get; set; }
        public User User { get; set; }
        public WareHouse WareHouse { get; set; }
        public ICollection<ProductSale> ProductSales { get; set; }

        #endregion
    }
}
