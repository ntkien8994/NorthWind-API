using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthWind.Entity;
using NorthWind.Library;
using System.Reflection;
using Newtonsoft.Json;
using static NorthWind.Library.Enumeration;

namespace NorthWind.API.Base
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Lấy tất cả danh sách theo của đối tượng
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <returns></returns>
        /// created by: ntkien 21.05.2020
        [HttpGet, Route("{entity}")]
        public virtual async Task<ServiceResult> GetAll(string entity)
        {

           ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _bl = Factory.CreateBL(entity);
                //nếu ko tìm thấy đối tượng BL tương ứng thì báo lỗi về cho client
                if (_bl == null)
                {
                    serviceResult.Message = Constant.SERVICE_ERROR_ENTITY_NOT_FOUND + " " + entity;
                    serviceResult.ResultCode = (int)ServiceResultCode.NotFound;
                    serviceResult.Success = false;
                }
                else
                {
                    var task = (Task)_bl.GetType().GetMethod("GetAll").Invoke(_bl, null);
                    await task.ConfigureAwait(false);
                    var data = task.GetType().GetProperty("Result").GetValue(task);
                    serviceResult.Data = JsonConvert.SerializeObject(data);
                    serviceResult.ResultCode = (int)ServiceResultCode.Success;
                }
            }
            catch (Exception ex)
            {
                serviceResult.Success = false;
                serviceResult.Message = ex.Message;
                serviceResult.ResultCode = (int)ServiceResultCode.InternalError;
            }
            return serviceResult;
        }
        
        /// <summary>
        /// Lấy đối tượng theo id
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        /// created by:ntkien 21.05.2020
        [HttpGet, Route("{entity}/{id}")]
        public virtual async Task<ServiceResult> GetByID(string entity, string id)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _bl = Factory.CreateBL(entity);
                //nếu ko tìm thấy đối tượng BL tương ứng thì báo lỗi về cho client
                if (_bl == null)
                {
                    serviceResult.Message = Constant.SERVICE_ERROR_ENTITY_NOT_FOUND + " " + entity;
                    serviceResult.ResultCode = (int)ServiceResultCode.NotFound;
                    serviceResult.Success = false;
                }
                else
                {
                    Guid objectId = Guid.Empty;
                    Guid.TryParse(id, out objectId);
                    if (objectId ==Guid.Empty)
                    {
                        var data = Factory.CreateEntity(entity);
                        Utility.SetValueForPrimaryKey(ref data);
                        serviceResult.Data = JsonConvert.SerializeObject(data);
                        serviceResult.ResultCode = (int)ServiceResultCode.Success;
                    }
                    else
                    {
                        var task = (Task)_bl.GetType().GetMethod("GetById").Invoke(_bl, new object[] { objectId });
                        await task.ConfigureAwait(false);
                        var data = task.GetType().GetProperty("Result").GetValue(task);
                        serviceResult.Data = JsonConvert.SerializeObject(data);
                        serviceResult.ResultCode = (int)ServiceResultCode.Success;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                serviceResult.Success = false;
                serviceResult.Message = ex.Message;
                serviceResult.ResultCode = (int)ServiceResultCode.InternalError;
            }

            return serviceResult;
        }

        /// <summary>
        /// Lấy danh sách chi tiết theo masterid
        /// </summary>
        /// <param name="entity">đối tượng</param>
        /// <param name="masterColumn">tên cột master</param>
        /// <param name="masterId">masterId</param>
        /// <returns></returns>
        /// created by:ntkien 21.05.2020
        [HttpGet, Route("{entity}/{masterColumn}/{masterId}")]
        public virtual async Task<ServiceResult> GetDetailByMasterID(string entity,string masterColumn, string masterId)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {
                var _bl = Factory.CreateBL(entity);
                //nếu ko tìm thấy đối tượng BL tương ứng thì báo lỗi về cho client
                if (_bl == null)
                {
                    serviceResult.Message = Constant.SERVICE_ERROR_ENTITY_NOT_FOUND + " " + entity;
                    serviceResult.ResultCode = (int)ServiceResultCode.NotFound;
                    serviceResult.Success = false;
                }
                else
                {
                    var task = (Task)_bl.GetType().GetMethod("GetDetailByMasterId").Invoke(_bl, new object[] { masterColumn, masterId});
                    await task.ConfigureAwait(false);
                    var data = task.GetType().GetProperty("Result").GetValue(task);
                    serviceResult.Data = JsonConvert.SerializeObject(data);
                    serviceResult.ResultCode = (int)ServiceResultCode.Success;
                }
            }
            catch (Exception ex)
            {
                serviceResult.Success = false;
                serviceResult.Message = ex.Message;
                serviceResult.ResultCode = (int)ServiceResultCode.InternalError;
            }

            return serviceResult;
        }

        [HttpPost, Route("{entity}")]
        public virtual async Task<ServiceResult> PostData(string entity, [FromBody] PostData postData)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //nếu param không hợp lệ thì báo lỗi về cho client
                if (postData == null || string.IsNullOrWhiteSpace(postData.MasterData))
                {
                    return new ServiceResult() { ResultCode = (int)ServiceResultCode.InternalError, Message = Constant.SERVICE_ERROR_PARAM_INVALID, Success = false };
                }

                var _bl = Factory.CreateBL(entity);
                //nếu ko tìm thấy đối tượng BL tương ứng thì báo lỗi về cho client
                if (_bl == null)
                {
                    return new ServiceResult() { ResultCode = (int)ServiceResultCode.NotFound, Message = Constant.SERVICE_ERROR_ENTITY_NOT_FOUND, Success = false };
                }

                var entityOject = Factory.CreateEntity(entity);
                var obj = JsonConvert.DeserializeObject(postData.MasterData, entityOject.GetType());
                PropertyInfo prop = _bl.GetType().GetProperty("UpdateData");
                // retrieves current List value to call Add method (IMPORTANT) 
                // THAM KHẢO https://stackoverflow.com/questions/39161851/how-to-add-an-object-to-a-generic-list-property-of-an-instance-of-a-class-using
                var datas = prop.GetValue(_bl);
                //find and call method add
                prop.PropertyType.GetMethod("Add").Invoke(datas, new[] { obj });

                //nếu có detail thì add vào
                if (postData.DetailData!=null &&postData.DetailData.Count>0)
                {
                    PropertyInfo propDetail = _bl.GetType().GetProperty("DetailData");
                    var detailDatas = propDetail.GetValue(_bl);
                    foreach(DetailObject item in postData.DetailData)
                    {
                        propDetail.PropertyType.GetMethod("Add").Invoke(detailDatas, new[] { item });
                    }
                }

                var task = (Task)_bl.GetType().GetMethod("SaveData").Invoke(_bl, null);
                await task.ConfigureAwait(false);
                var data = task.GetType().GetProperty("Result").GetValue(task);
                serviceResult.Data = JsonConvert.SerializeObject(data);
                serviceResult.ResultCode = (int)ServiceResultCode.Success;

            }
            catch (Exception ex)
            {
                serviceResult.Success = false;
                serviceResult.Message = ex.Message;
                serviceResult.ResultCode = (int)ServiceResultCode.InternalError;
            }

            return serviceResult;
        }

        [HttpPost, Route("{entity}/paging")]
        public virtual async Task<ServiceResult> Paging(string entity, [FromBody] PagingObject paging)
        {

            ServiceResult serviceResult = new ServiceResult();
            try
            {
                //nếu param không hợp lệ thì báo lỗi về cho client
                if (paging == null)
                {
                    return new ServiceResult() { ResultCode = (int)ServiceResultCode.InternalError, Message = Constant.SERVICE_ERROR_PARAM_INVALID, Success = false };
                }

                var _bl = Factory.CreateBL(entity);
                //nếu ko tìm thấy đối tượng BL tương ứng thì báo lỗi về cho client
                if (_bl == null)
                {
                    return new ServiceResult() { ResultCode = (int)ServiceResultCode.NotFound, Message = Constant.SERVICE_ERROR_ENTITY_NOT_FOUND, Success = false };
                }

                PropertyInfo prop = _bl.GetType().GetProperty("PagingObject");
                prop.SetValue(_bl, paging);

                var task = (Task)_bl.GetType().GetMethod("GetByPaging").Invoke(_bl, null);
                await task.ConfigureAwait(false);
                var data = task.GetType().GetProperty("Result").GetValue(task);
                serviceResult.Data = JsonConvert.SerializeObject(data);
                serviceResult.ResultCode = (int)ServiceResultCode.Success;

            }
            catch (Exception ex)
            {
                serviceResult.Success = false;
                serviceResult.Message = ex.Message;
                serviceResult.ResultCode = (int)ServiceResultCode.InternalError;
            }

            return serviceResult;
        }
    }
}