using System;
using System.Collections.Generic;

namespace NorthWind.Entity
{
    public partial class News : EntityBase
    {
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
