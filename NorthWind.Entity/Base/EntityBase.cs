using System;
using System.ComponentModel.DataAnnotations.Schema;
using NorthWind.Library;

namespace NorthWind.Entity
{
    public class EntityBase
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped]
        public Enumeration.EditMode EditMode { get; set; }

        //public string Count { get; set; }
    }
}
