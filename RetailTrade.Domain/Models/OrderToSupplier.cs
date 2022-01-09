using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class OrderToSupplier : DomainObject
    {
        #region Private Members

        private int _orderStatusId;

        #endregion

        #region Public Properties

        public DateTime OrderDate { get; set; }
        public int SupplierId { get; set; }
        public int OrderStatusId
        {
            get => _orderStatusId;
            set
            {
                _orderStatusId = value;
                OnPropertyChanged(nameof(OrderStatusId));
            }
        }
        public string Comment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Sum { get; set; }
        public Supplier Supplier { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }

        #endregion
    }
}
