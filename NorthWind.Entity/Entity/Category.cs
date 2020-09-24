using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthWind.Entity
{
    [Table("Category")]
    public partial class Category : EntityBase
    {
        [Key()]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? CategoryType { get; set; }
        public Guid? ParentId { get; set; }
        public string TreeCode { get; set; }
        public bool? Inactive { get; set; }
        public string Metadata { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
    }
}
