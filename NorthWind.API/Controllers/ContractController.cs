using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Entity;
using NorthWind.Library;
using System.Reflection;
using Newtonsoft.Json;
using NorthWind.BL;

namespace NorthWind.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractController : Controller
    {
        /// <summary>
        /// Lấy danh sách chi tiết theo masterid
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <param name="masterColumn">tên cột master</param>
        /// <param name="masterId">masterId</param>
        /// <returns></returns>
        /// created by:ntkien 21.05.2020
        [HttpGet, Route("GetDetailViewByMaster/{masterId}")]
        public virtual async Task<ServiceResult> GetDetailViewByMaster(string masterId)
        {



            ServiceResult serviceResult = new ServiceResult();
            try
            {

                BLContractDetail bl = new BLContractDetail();
                var data = await bl.GetDetailViewByMasterId(masterId);
                serviceResult.Data = JsonConvert.SerializeObject(data);
                serviceResult.ResultCode = (int)Enumeration.ServiceResultCode.Success;
            }
            catch (Exception ex)
            {
                serviceResult.Success = false;
                serviceResult.Message = ex.Message;
                serviceResult.ResultCode = (int)Enumeration.ServiceResultCode.InternalError;
            }

            return serviceResult;
        }
    }
}
