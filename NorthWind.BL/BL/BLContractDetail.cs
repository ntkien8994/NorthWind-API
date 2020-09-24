using NorthWind.DL;
using NorthWind.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using NorthWind.Library;
using static NorthWind.Library.Enumeration;

namespace NorthWind.BL
{
    public class BLContractDetail : BLBase<ContractDetail>
    {
        public async override Task<List<ContractDetail>> GetDetailByMasterId(string masterColumn, string masterId)
        {
            List<ContractDetail> result = null;
            using (NorthWindContext<ContractDetail> context = new NorthWindContext<ContractDetail>())
            {
                result = await context.ContractDetails.FromSqlRaw("Exec [dbo].[Proc_GetContractDetail_ById] @ContractId={0}", masterId)
                    .ToListAsync();
            }
            return result;
        }
    }
}
