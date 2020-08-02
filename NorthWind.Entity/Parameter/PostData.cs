using System;
using System.Collections.Generic;
using System.Text;
using static NorthWind.Library.Enumeration;

/// <summary>
/// class dùng để chứa dữ liệu đẩy từ dưới lên
/// </summary>
/// created by: ntkien 21.05.2020
namespace NorthWind.Entity
{
    public class PostData
    {
        public string MasterData { get; set; }
        public List<DetailObject> DetailData { get; set; }
    }
    /// <summary>
    /// đối tượng chứa các giá trị detail của master khi post dữ liệu
    /// </summary>
    /// created by: ntkien 02.06.2020
    public class DetailObject
    {
        /// <summary>
        /// tên bảng
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// giá trị
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// đối tượng chứa các điều kiện filter
    /// </summary>
    /// created by: ntkien 02.06.2020
    public class WhereObject
    {
        /// <summary>
        /// tên cột
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Giá trị
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// nếu so sánh nằm trong khoảng thì bổ sung thêm giá trị Value2
        /// </summary>
        public string Value2 { get; set; }
        public ExpressionOperationEnum Operation { get; set; }
        public ColumnTypeEnum ColumnType { get; set; }
    }
    /// <summary>
    /// đối tượng chứa các giá trị sort 
    /// </summary>
    /// created by: ntkien 02.06.2020
    public class OrderByObject
    {
        public string ColumnName { get; set; }
        /// <summary>
        /// ASC / DESC
        /// </summary>
        public string SortOperation { get; set; }
    }

    /// <summary>
    ///đối tượng để gọi paging
    /// </summary>
    /// created by: ntkien 02.06.2020
    public class PagingObject
    {
        public string PrimaryKey { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public List<OrderByObject> OrderInfos { get; set; }
        public List<WhereObject> WhereInfos { get; set; }
    }

    /// <summary>
    /// đối tượng chứa kết quả paging
    /// </summary>
    /// created by: ntkien 02.06.2020
    public class PagingResult<T> where T : EntityBase, new()
    {
        /// <summary>
        /// dữ liệu trả về
        /// </summary>
        public List<T> Data { get; set; }
        //tổng số bản ghi
        public int TotalCount { get; set; }
    }
}
