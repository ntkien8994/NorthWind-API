using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthWind.Entity
{
    [Table("News")]
    public partial class News : EntityBase
    {
        [Key()]
        public Guid NewsId { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public Guid? CategoryId { get; set; }
        public string Content { get; set; }
        public string Decription { get; set; }
        public string Keyword { get; set; }
        public string Meta { get; set; }
        public bool? Inactive { get; set; }
        public string ImageUrl { get; set; }
    }
}
