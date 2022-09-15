using System;

namespace RetailTrade.Domain.Models
{
    public class WriteDown : DomainObject
    {
        public DateTime WriteDownDate { get; set; }
        public int SupplierId { get; set; }
        public string Comment { get; set; }
        public Supplier Supplier { get; set; }
    }
}
