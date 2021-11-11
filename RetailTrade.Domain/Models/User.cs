using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class User : DomainObject
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime? JoinedDate { get; set; }

        /// <summary>
        /// Код ролья
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Филалы
        /// </summary>
        public Branch Branch { get; set; }

        /// <summary>
        /// Список смен
        /// </summary>
        public ICollection<Shift> Shifts { get; set; }
    }
}
