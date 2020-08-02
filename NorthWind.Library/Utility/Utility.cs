using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using static NorthWind.Library.Enumeration;
using System.Reflection;

namespace NorthWind.Library
{
    /// <summary>
    /// class tiện ích chung của cả ứng dụng
    /// </summary>
    /// created by: ntkien 20.05.2020
    public static class Utility
    {
        /// <summary>
        /// hàm dùng để convert giá trị từ string sang đúng type
        /// </summary>
        /// <param name="value">giá trị kiểu string</param>
        /// <param name="columnType">giá trị kiểu type</param>
        /// <returns></returns>
        /// created by: ntkien 03.06.2020
        public static object ConvertValueByType(string value, ColumnTypeEnum columnType)
        {
            object result = null;
            switch (columnType)
            {
                case ColumnTypeEnum.Boolean:
                    result = default(bool);
                    bool boolValue;
                    bool.TryParse(value, out boolValue);
                    result = boolValue;
                    break;
                case ColumnTypeEnum.DateTime:
                    result = default(DateTime);
                    DateTime dateValue;
                    DateTime.TryParse(value, out dateValue);
                    result = dateValue;
                    break;
                case ColumnTypeEnum.Int:
                    result = default(int);
                    int iValue;
                    int.TryParse(value, out iValue);
                    result = iValue;
                    break;
                case ColumnTypeEnum.Decimal:
                    result = default(decimal);
                    decimal decimalValue;
                    decimal.TryParse(value, out decimalValue);
                    result = decimalValue;
                    break;
                case ColumnTypeEnum.Guid:
                    result = default(Guid);
                    Guid guidValue;
                    Guid.TryParse(value, out guidValue);
                    result = guidValue;
                    break;
                case ColumnTypeEnum.String:
                    result = (value != null ? value.ToString() : "");
                    break;
            }
            return result;
        }

        /// <summary>
        /// hàm dùng để convert giá trị từ string sang đúng type
        /// </summary>
        /// <param name="value">giá trị kiểu string</param>
        /// <param name="columnType">giá trị kiểu type</param>
        /// <returns></returns>
        /// created by: ntkien 03.06.2020
        public static object ConvertValueByType(string value, string columnType)
        {
            object result = null;
            switch (columnType.ToLower())
            {
                case "boolean":
                    result = default(bool);
                    bool boolValue;
                    bool.TryParse(value, out boolValue);
                    result = boolValue;
                    break;
                case "datetime":
                    result = default(DateTime);
                    DateTime dateValue;
                    DateTime.TryParse(value, out dateValue);
                    result = dateValue;
                    break;
                case "int":
                    result = default(int);
                    int iValue;
                    int.TryParse(value, out iValue);
                    result = iValue;
                    break;
                case "decimal":
                    result = default(decimal);
                    decimal decimalValue;
                    decimal.TryParse(value, out decimalValue);
                    result = decimalValue;
                    break;
                case "guid":
                    result = default(Guid);
                    Guid guidValue;
                    Guid.TryParse(value, out guidValue);
                    result = guidValue;
                    break;
                case "string":
                    result = (value != null ? value.ToString() : "");
                    break;
            }
            return result;
        }

        public static MethodInfo GetMethodInfo<T1, T2, T3>(
            Func<T1, T2, T3> f,
            T1 unused1,
            T2 unused2)
        {
            return f.Method;
        }

    }
}
