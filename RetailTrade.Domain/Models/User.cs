using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class User : DomainObject
    {
        #region Private Members

        private string _username;
        private string _fullname;
        private string _passwordHash;
        private DateTime? _joinedDate;
        private int _roleId;
        private bool _deleteMark;
        private int? _wareHouseId;

        #endregion

        #region Public Properties

        /// <summary>
        /// Логин
        /// </summary>
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName
        {
            get => _fullname;
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                _passwordHash = value;
                OnPropertyChanged(nameof(PasswordHash));
            }
        }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime? JoinedDate
        {
            get => _joinedDate;
            set
            {
                _joinedDate = value;
                OnPropertyChanged(nameof(JoinedDate));
            }
        }

        /// <summary>
        /// Код ролья
        /// </summary>
        public int RoleId
        {
            get => _roleId;
            set
            {
                _roleId = value;
                OnPropertyChanged(nameof(RoleId));
            }
        }

        /// <summary>
        /// Код точки продажи
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
        /// Пометка на удаление, по умолчанию false
        /// </summary>
        public bool DeleteMark
        {
            get => _deleteMark;
            set
            {
                _deleteMark = value;
                OnPropertyChanged(nameof(DeleteMark));
            }
        }

        /// <summary>
        /// Роль
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Филалы
        /// </summary>
        public Branch Branch { get; set; }

        /// <summary>
        /// Точка продажи
        /// </summary>
        public WareHouse WareHouse { get; set; }

        /// <summary>
        /// Список смен
        /// </summary>
        public ICollection<Shift> Shifts { get; set; }

        #endregion
    }
}
