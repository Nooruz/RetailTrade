using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class Arrival : DomainObject
    {
        public DateTime ArrivalDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int SupplierId { get; set; }
        public string Comment { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<ArrivalProduct> ArrivalProducts { get; set; }
    }
}
