using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthWind.Entity
{
    [Table("Order")]
    public partial class Order:EntityBase
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        [Key()]
        public Guid OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Vatrate { get; set; }
        public decimal? Vatamount { get; set; }
        public Guid? CustomId { get; set; }
        public string Address { get; set; }
        public int? PaymentType { get; set; }
        public bool? IsPaid { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
