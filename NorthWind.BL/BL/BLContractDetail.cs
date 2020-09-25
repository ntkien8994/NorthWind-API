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
        public async Task<List<ContractDetailView>> GetDetailViewByMasterId(string masterId)
        {
            using (NorthWindContext<ContractDetailView> context = new NorthWindContext<ContractDetailView>())
            {
                return await context.ContractDetailsView.FromSqlRaw("Exec [dbo].[Proc_GetContractDetail_ById] @ContractId={0}", masterId)
                    .ToListAsync();
            }
        }
    }
}
