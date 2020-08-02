using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Library
{
    /// <summary>
    /// class chứa tất cả các enum của chương trình
    /// </summary>
    /// created by: ntkien 11.05.2020
    public static class Enumeration
    {
        /// <summary>
        /// trạng thái của entity
        /// </summary>
        public enum EditMode
        {
            None = 0,
            Add = 1,
            Edit = 2,
            Delete = 3
        }

        /// <summary>
        /// Mã kết quả của service trả về cho client
        /// </summary>
        public enum ServiceResultCode
        {
            Success=200,
            UnAuthorize = 401,
            NotFound = 404,
            InternalError = 500
        }

        /// <summary>
        /// Operation
        /// </summary>
        public enum ExpressionOperationEnum
        {
            Equals = 1,
            Contains = 2,
            GreatThan = 3,
            GreatThanEqual = 4,
            LessThan = 5,
            LessThanEqual = 6
        }

        /// <summary>
        /// column Type
        /// </summary>
        public enum ColumnTypeEnum
        {
            String = 0,
            Int = 1,
            Decimal = 2,
            Boolean = 3,
            DateTime = 4,
            Guid = 5
        }
    }
}
