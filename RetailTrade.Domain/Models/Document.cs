﻿using System;
using System.Collections.ObjectModel;

namespace RetailTrade.Domain.Models
{
    public class Document : DomainObject
    {
        #region Private Members

        private int _documentTypeId;
        private DateTime _createdDate = DateTime.Now;
        private int _userId;
        private decimal _amount;
        private int? _wareHouseId;
        private int? _toWareHouseId;
        private string _number;
        private string _comment;
        private ObservableCollection<DocumentProduct> _documentProducts = new();

        #endregion

        #region Public Properties

        /// <summary>
        /// Код тип документа
        /// </summary>
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

        /// <summary>
        /// Со склад
        /// </summary>
        public int? WareHouseId
        {
            get => _wareHouseId;
            set
            {
                _wareHouseId = value;
                OnPropertyChanged(nameof(WareHouseId));
            }
        }

        /// <summary>
        /// На склада
        /// </summary>
        public int? ToWareHouseId
        {
            get => _toWareHouseId;
            set
            {
                _toWareHouseId = value;
                OnPropertyChanged(nameof(ToWareHouseId));
            }
        }
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
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
        public ObservableCollection<DocumentProduct> DocumentProducts
        {
            get => _documentProducts;
            set
            {
                _documentProducts = value;
                OnPropertyChanged(nameof(DocumentProducts));
            }
        }

        #endregion
    }
}
