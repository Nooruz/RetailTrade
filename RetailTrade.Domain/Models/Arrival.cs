using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class Arrival : DomainObject
    {
        public DateTime ArrivalDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int SupplierId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum { get; set; }
        public string Comment { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<ArrivalProduct> ArrivalProducts { get; set; }
    }
}
