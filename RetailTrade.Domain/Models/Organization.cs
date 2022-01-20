namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// Текущая организация
    /// </summary>
    public class Organization : DomainObject
    {
        /// <summary>
        /// Полное наименование организации
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Сокращенное наименование организации
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Полное наименование организации
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// ФИО директора
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Номер телефона организации
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// ИНН организации
        /// </summary>
        public string Inn { get; set; }
    }
}
