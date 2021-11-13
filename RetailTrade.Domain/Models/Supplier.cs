using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Supplier : DomainObject
    {
        /// <summary>
        /// Полное наименование поставщика
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Краткое наименование поставщика
        /// </summary>
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Inn { get; set; }
        public DateTime CreateDate { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<OrderToSupplier> Orders { get; set; }
    }
}
