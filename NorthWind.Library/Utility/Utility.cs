using System;
using System.Linq;
using static NorthWind.Library.Enumeration;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Newtonsoft.Json;
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
        public static object ConvertValueByType(object value, string columnType)
        {
            object result = null;
            switch (columnType.ToLower())
            {
                case "boolean":
                    result = default(bool);
                    bool boolValue;
                    bool.TryParse(value.ToString(), out boolValue);
                    result = boolValue;
                    break;
                case "datetime":
                    result = default(DateTime);
                    DateTime dateValue;
                    //DateTime.TryParse(value, out dateValue);
                    DateTime.TryParseExact(value.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    result = dateValue;
                    break;
                case "int32":
                    result = JsonConvert.DeserializeObject<Int32>(value.ToString());
                    break;
                case "int64":
                    result = JsonConvert.DeserializeObject<Int64>(value.ToString());
                    break;
                case "int":
                    result = default(int);
                    int iValue;
                    int.TryParse(value.ToString(), out iValue);
                    result = iValue;
                    break;
                case "decimal":
                    result = JsonConvert.DeserializeObject<decimal>(value.ToString());
                    break;
                case "guid":
                    result = default(Guid);
                    Guid guidValue;
                    Guid.TryParse(value.ToString(), out guidValue);
                    result = guidValue;
                    break;
                case "string":
                    result = (value != null ? value.ToString() : "");
                    break;
            }
            return result;
        }
        /// <summary>
        /// hàm kiểm tra xem có phải là kiểu nullable ko
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// created by: ntkien 04.09.2020
        public static bool IsNullableType(Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        public static MethodInfo GetMethodInfo<T1, T2, T3>(
            Func<T1, T2, T3> f,
            T1 unused1,
            T2 unused2)
        {
            return f.Method;
        }

        /// <summary>
        /// hàm thực hiện lấy tên trường khóa chính
        /// </summary>
        /// <param name="obj">đối tượng</param>
        /// <returns></returns>
        /// created by: ntkien 29.08.2020
        public static string GetPrimaryKeyName(object obj)
        {
            string result = "";
            if (obj==null)
            {
                return "";
            }
            var properties = obj.GetType().GetProperties();
            if (properties !=null && properties.Length>0)
            {
                foreach (var prop in properties)
                {
                    var customAttrs = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(KeyAttribute));
                    if (customAttrs!=null)
                    {
                        result = prop.Name;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// hàm thực hiện lấy tên chuẩn đã đặt trong entity
        /// </summary>
        /// <param name="obj">đối tượng</param>
        /// <returns></returns>
        /// created by: ntkien 23.09.2020
        public static string GetStandardPropertyName(object obj,string propertyName)
        {
            string result = "";
            if (obj == null)
            {
                return "";
            }
            var properties = obj.GetType().GetProperties();
            if (properties != null && properties.Length > 0)
            {
                foreach (var prop in properties)
                {
                    if (prop.Name.Equals(propertyName,StringComparison.OrdinalIgnoreCase))
                    {
                        result = prop.Name;
                        break;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// hàm thực hiện set giá trị cho trường khóa chính
        /// </summary>
        /// <param name="obj"></param>
        /// created by: ntkien 30.08.2020
        public static void SetValueForPrimaryKey(ref object obj)
        {
            if (obj==null)
            {
                return;
            }
            string primaryKeyName = GetPrimaryKeyName(obj);
            PropertyInfo prop = obj.GetType().GetProperty(primaryKeyName);
            switch(prop.PropertyType.Name.ToLower())
            {
                case "guid":
                    prop.SetValue(obj, Guid.NewGuid());
                    break;
            }
        }
    }
}
