using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthWind.Entity
{
    [Table("Product")]
    public partial class Product : EntityBase
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Content { get; set; }
        public decimal? UnitPrice { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? Inactive { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
