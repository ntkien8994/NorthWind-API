using NorthWind.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// interface cho BLBase
/// </summary>
/// created by: ntkien 17.05.2020
namespace NorthWind.BL
{
    interface IBLBase<T> where T : EntityBase, new()
    {
        /// <summary>
        /// get all
        /// </summary>
        Task<List<T>> GetAll();
        /// <summary>
        /// get by id
        /// </summary>
        /// <param name="id"></param>
        Task<T> GetById(object id);
        /// <summary>
        /// get paging
        /// </summary>
        Task<PagingResult<T>> GetByPaging();
        /// <summary>
        /// get detail by master id
        /// </summary>
        Task<List<T>> GetDetailByMasterId(string masterColumn, string masterId);
        /// <summary>
        /// post data
        /// </summary>
        Task<int> SaveData();
    }
}
