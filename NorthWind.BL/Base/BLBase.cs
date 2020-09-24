using System;
using System.Collections.Generic;
using System.Data;
using NorthWind.DL;
using NorthWind.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthWind.Library;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using System.Linq.Expressions;
using static NorthWind.Library.Enumeration;

namespace NorthWind.BL
{
    /// <summary>
    /// BL Base 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// created by: ntkien 11.05.2020
    public class BLBase<T> : IBLBase<T> where T : EntityBase, new()
    {
        #region Declare
        /// <summary>
        /// Danh sách thay đổi dữ liệu
        /// </summary>
        private List<T> _UpdateData;
        public List<T> UpdateData
        {
            get
            {
                return _UpdateData;
            }
            set
            {
                _UpdateData = value;
            }
        }

        /// <summary>
        /// list dùng để chứa các dữ liệu detail
        /// </summary>
        private List<DetailObject> _DetailData;
        public List<DetailObject> DetailData
        {
            get
            {
                return _DetailData;
            }
            set
            {
                _DetailData = value;
            }
        }

        /// <summary>
        /// chứa đối tượng paging
        /// </summary>
        private PagingObject _PagingObject;
        public PagingObject PagingObject
        {
            get
            {
                return _PagingObject;
            }
            set
            {
                _PagingObject = value;
            }
        }
        #endregion
        #region Contructor
        public BLBase()
        {
            _UpdateData = new List<T>();
            _DetailData = new List<DetailObject>();
        }
        #endregion
        #region Sub/Function
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns></returns>
        /// created by: ntkien 11.05.2020
        public virtual async Task<List<T>> GetAll()
        {
            List<T> results = null;
            using (NorthWindContext<T> context = new NorthWindContext<T>())
            {
                results = await context.ListBase.ToListAsync();
            }
            return results;
        }

        /// <summary>
        /// Get paging
        /// </summary>
        /// <returns></returns>
        /// created by: ntkien 02.06.2020
        public virtual async Task<PagingResult<T>> GetByPaging()
        {
            List<T> results = null;
            int totalCounts = 0;
            using (NorthWindContext<T> context = new NorthWindContext<T>())
            {
                IQueryable<T> query = context.ListBase;
                IQueryable<T> resultQuery = context.ListBase;
                if (_PagingObject == null)
                {
                    return null;
                }
                if (_PagingObject.WhereInfos != null && _PagingObject.WhereInfos.Count > 0)
                {
                    foreach (WhereObject whereObject in _PagingObject.WhereInfos)
                    {
                        //ntkien: nếu là toán tử bettween thì sẽ apply where 2 lần
                        if (whereObject.Operation==ExpressionOperationEnum.Bettween)
                        {
                            query = query.ApplyWhere(whereObject.ColumnName, whereObject.Value, whereObject.ColumnType, ExpressionOperationEnum.GreatThanEqual);
                            query = query.ApplyWhere(whereObject.ColumnName, whereObject.Value2, whereObject.ColumnType, ExpressionOperationEnum.LessThanEqual);
                        }
                        else
                        {
                            query = query.ApplyWhere(whereObject.ColumnName, whereObject.Value, whereObject.ColumnType, whereObject.Operation);
                        }    
                    }
                }
                if (_PagingObject.OrderInfos != null && _PagingObject.OrderInfos.Count > 0)
                {
                    bool isFirst = true;
                    foreach (OrderByObject orderByObject in _PagingObject.OrderInfos)
                    {
                        string sort = orderByObject.SortOperation;
                        if (!string.IsNullOrWhiteSpace(sort) && sort.Equals("desc", StringComparison.OrdinalIgnoreCase))
                        {
                            if (isFirst)
                            {
                                query = query.ApplyOrderByDesc(orderByObject.ColumnName);
                            }
                            else
                            {
                                query = query.ApplyThenByDesc(orderByObject.ColumnName);
                                isFirst = false;
                            }
                        }
                        else
                        {
                            if (isFirst)
                            {
                                query = query.ApplyOrderBy(orderByObject.ColumnName);
                            }
                            else
                            {
                                query = query.ApplyThenBy(orderByObject.ColumnName);
                                isFirst = false;
                            }
                        }
                    }
                }
                //ntkien 04.06.2020 lấy tổng số bản ghi
                totalCounts = await query.CountAsync();
                if (_PagingObject.Skip >= 0)
                {
                    query = query.Skip(_PagingObject.Skip).Take(_PagingObject.Take);
                }
                //ntkien 04.06.2020 nếu khóa chính ko truyền vào thì lấy theo định dạng EntityName + Id
                T t = new T();
                var primayKey = Utility.GetPrimaryKeyName(t);
                if (string.IsNullOrWhiteSpace(primayKey))
                {
                    primayKey = _PagingObject.PrimaryKey;
                    if (string.IsNullOrWhiteSpace(primayKey))
                    {
                        primayKey = string.Format("{0}Id", t.GetType().Name);
                    }
                }
                var x = Expression.Parameter(typeof(T), "x");
                var body = Expression.PropertyOrField(x, primayKey);
                var lambda = Expression.Lambda<Func<T, Guid>>(body, x);

                //ntkien 04.06.2020 lấy ra danh sách id rồi mới lấy ra đầy đủ bản ghi (mục đích dùng để tối ưu)
                var ids = await query.Select(lambda).ToListAsync();
                if (ids != null && ids.Count > 0)
                {
                    Expression expressions = null;
                    var mba = PropertyAccessorCache<T>.Get(primayKey);
                    Expression eqe;
                    foreach (object id in ids)
                    {
                        eqe = Expression.Equal(mba.Body, Expression.Constant(id, mba.ReturnType));
                        if (expressions == null)
                        {
                            expressions = eqe;
                        }
                        else
                        {
                            expressions = Expression.OrElse(expressions, eqe);
                        }
                    }
                    var expression = Expression.Lambda(expressions, mba.Parameters[0]);

                    // 4. Construct new query
                    MethodCallExpression resultExpression = Expression.Call(
                        null,
                        Utility.GetMethodInfo(Queryable.Where, resultQuery, (Expression<Func<T, bool>>)null),
                        new Expression[] { resultQuery.Expression, Expression.Quote(expression) });
                    resultQuery = resultQuery.Provider.CreateQuery<T>(resultExpression);



                    if (_PagingObject.OrderInfos != null && _PagingObject.OrderInfos.Count > 0)
                    {
                        bool isFirst = true;
                        foreach (OrderByObject orderByObject in _PagingObject.OrderInfos)
                        {
                            string sort = orderByObject.SortOperation;
                            if (!string.IsNullOrWhiteSpace(sort) && sort.Equals("desc", StringComparison.OrdinalIgnoreCase))
                            {
                                if (isFirst)
                                {
                                    resultQuery = resultQuery.ApplyOrderByDesc(orderByObject.ColumnName);
                                }
                                else
                                {
                                    resultQuery = resultQuery.ApplyThenByDesc(orderByObject.ColumnName);
                                    isFirst = false;
                                }
                            }
                            else
                            {
                                if (isFirst)
                                {
                                    resultQuery = resultQuery.ApplyOrderBy(orderByObject.ColumnName);
                                }
                                else
                                {
                                    resultQuery = resultQuery.ApplyThenBy(orderByObject.ColumnName);
                                    isFirst = false;
                                }
                            }
                        }
                    }

                    results = await resultQuery.ToListAsync();
                }

            }

            return new PagingResult<T>() { Data = results, TotalCount = totalCounts };
        }

        /// <summary>
        /// get object by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// created by: ntkien 11.05.2020
        public virtual async Task<T> GetById(object id)
        {
            T result = null;
            using (NorthWindContext<T> context = new NorthWindContext<T>())
            {
                result = await context.ListBase.FindAsync(id);
            }
            return result;
        }

        /// <summary>
        /// get list detail by masterid
        /// </summary>
        /// <param name="masterId">id của master</param>
        /// <returns></returns>
        /// created by: ntkien 24.05.2020
        public virtual async Task<List<T>> GetDetailByMasterId(string masterColumn, string masterId)
        {
            List<T> result = null;
            using (NorthWindContext<T> context = new NorthWindContext<T>())
            {
                result = await context.ListBase.ApplyWhere(masterColumn, masterId, ColumnTypeEnum.String, ExpressionOperationEnum.Equals).ToListAsync();
            }
            return result;
        }

        /// <summary>
        /// hàm submit data
        /// </summary>
        /// <returns></returns>
        /// created by:ntkien 28.05.2020
        public virtual async Task<int> SaveData()
        {
            int effects = 0;
            using (NorthWindContext<T> context = new NorthWindContext<T>())
            {
                foreach (T item in _UpdateData)
                {
                    if (item.EditMode == Enumeration.EditMode.Add)
                    {
                        //thực hiện set ngày ở đây
                        item.CreatedDate = DateTime.Now;
                        item.ModifiedDate = DateTime.Now;
                        context.ListBase.Add(item);
                    }
                    else if (item.EditMode == Enumeration.EditMode.Edit)
                    {
                        //thực hiện set ngày ở đây
                        item.ModifiedDate = DateTime.Now;
                        context.ListBase.Update(item);
                    }
                    else if (item.EditMode == Enumeration.EditMode.Delete)
                    {
                        context.ListBase.Remove(item);
                    }
                }
                //nếu có detail
                if (DetailData != null && DetailData.Count > 0)
                {
                    foreach (DetailObject detail in DetailData)
                    {
                        var entity = EntityFactory.CreateEntity(detail.TableName);
                        if (entity == null)
                        {
                            continue;
                        }
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(entity.GetType());
                        var details = (IList)Activator.CreateInstance(constructedListType);

                        var method = context.GetType().GetMethods()
                           .First(x => x.IsGenericMethod && x.Name == "Set");
                        MethodInfo generic = method.MakeGenericMethod(entity.GetType());
                        var dbSet = generic.Invoke(context, null);

                        var list = (IList)JsonConvert.DeserializeObject(detail.Value, details.GetType());
                        foreach (var obj in list)
                        {
                            var property = obj.GetType().GetProperty("EditMode");
                            if (property == null)
                            {
                                continue;
                            }
                            var value = property.GetValue(obj);
                            if (value == null)
                            {
                                continue;
                            }
                            var editMode = (Enumeration.EditMode)value;
                            if (editMode == Enumeration.EditMode.Add)
                            {
                                //ntkien: 23.09.2020 để tối ưu thì sẽ set lại value cho khóa chính ở đây.
                                // vì ko referen  gì nên thay đổi value khóa chính ko vấn đề gì
                                //sau khi save xong đã reload lại rồi nên sẽ lấy dữ liệu mới nhất trong database
                                var objAdd = obj;
                                var primayKey = Utility.GetPrimaryKeyName(objAdd);
                                Utility.SetValueForPrimaryKey(ref objAdd);
                                dbSet.GetType().GetMethod("Add").Invoke(dbSet, new object[] { objAdd });
                            }
                            else if (editMode == Enumeration.EditMode.Edit)
                            {
                                dbSet.GetType().GetMethod("Update").Invoke(dbSet, new object[] { obj });
                            }
                            else if (editMode == Enumeration.EditMode.Delete)
                            {
                                dbSet.GetType().GetMethod("Remove").Invoke(dbSet, new object[] { obj });
                            }
                        }
                    }
                }
                effects = await context.SaveChangesAsync();
                return effects;
            }
        }
        #endregion
        #region Event
        #endregion

    }
}
