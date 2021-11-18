using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class RefundToSupplier : DomainObject
    {
        public DateTime RefundToSupplierDate { get; set; }
        public int SupplierId { get; set; }
        public string Comment { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<RefundToSupplierProduct> RefundToSupplierProducts { get; set; }
    }
}
