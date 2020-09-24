using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthWind.Entity
{
    [Table("Contract")]
    public class Contract : EntityBase
    {
        public Contract()
        {
            ContractDetail = new HashSet<ContractDetail>();
        }

        [Key()]
        public Guid ContractId { get; set; }

        public string ContractCode { get; set; }

        public Guid? CustomerId { get; set; }

        public DateTime? ContractDate { get; set; }

        public int? ContactType { get; set; }

        public string ContactName { get; set; }

        public string ContactTel { get; set; }

        public string CompanyName { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public decimal ContractAmount { get; set; }
        public virtual ICollection<ContractDetail> ContractDetail { get; set; }

    }
}
