using System;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee : DomainObject
    {
        #region Private Members

        private string _fullName;
        private int _genderId;
        private DateTime _birthDate;
        private string _inn;

        #endregion

        #region Public Properties

        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Пол
        /// </summary>
        public int GenderId
        {
            get => _genderId;
            set
            {
                _genderId = value;
                OnPropertyChanged(nameof(GenderId));
            }
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        /// <summary>
        /// ИНН
        /// </summary>
        public string Inn
        {
            get => _inn;
            set
            {
                _inn = value;
                OnPropertyChanged(nameof(Inn));
            }
        }

        public int GroupEmployeeId { get; set; }

        /// <summary>
        /// Гендер
        /// </summary>
        public Gender Gender { get; set; }
        public GroupEmployee GroupEmployee { get; set; }

        #endregion
    }
}
