using System;

namespace RetailTrade.Domain.Views
{
    public class DocumentView : ViewObject
    {
        #region Private Members

        private int _documentTypeId;
        private DateTime _createdDate;
        private string _username;
        private decimal _amount;
        private string _wareHouse;
        private string _comment;
        private string _number;
        private string _toWareHouse;

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
        public string Username 
        { 
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public decimal Amount 
        { 
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string WareHouse 
        { 
            get => _wareHouse;
            set
            {
                _wareHouse = value;
                OnPropertyChanged(nameof(WareHouse));
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
        public string Number 
        { 
            get => _number; 
            set
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        public string ToWareHouse 
        { 
            get => _toWareHouse;
            set
            {
                _toWareHouse = value;
                OnPropertyChanged(nameof(ToWareHouse));
            }
        }

        #endregion
    }
}
