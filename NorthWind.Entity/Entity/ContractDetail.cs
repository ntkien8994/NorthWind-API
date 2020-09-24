using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NorthWind.Entity
{
    [Table("ContractDetail")]
    public class ContractDetail : EntityBase
    {
        [Key()]
        public Guid ContractDetailId { get; set; }

        public Guid? ContractId { get; set; }

        public Guid? ProductId { get; set; }

        public string ProductCode { get; set; }

        //[NotMapped]
        public string ProductName { get; set; }

        public decimal? UnitPrice { get; set; }

        public int? Quantity { get; set; }

        public decimal? Amount { get; set; }

        public decimal? PromotionRate { get; set; }

        public decimal? TotalAmount { get; set; }

    }
}
