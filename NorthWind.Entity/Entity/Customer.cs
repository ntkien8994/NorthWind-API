using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthWind.Entity
{
    [Table("Customer")]
    public partial class Customer:EntityBase
    {
        [Key()]
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int? Age { get; set; }
        public Decimal? Revenue { get; set; }
        public bool? Inactive { get; set; }
    }
}
