using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthWind.Entity
{
    [Table("Product")]
    public partial class Product : EntityBase
    {
        [Key()]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal? UnitPrice { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? Inactive { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? InputDate { get; set; }
        public string Description { get; set; }
    }
}
