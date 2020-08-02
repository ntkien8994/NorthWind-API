using NorthWind.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using NorthWind.BL;

/// <summary>
/// Factory
/// </summary>
/// created by: ntkien 20.05.2020
namespace NorthWind.API
{
    public static class Factory
    {
        /// <summary>
        /// khởi tạo đối tượng BL tương ứng với entity name 
        /// </summary>
        /// <param name="entity">tên của entity</param>
        /// <returns></returns>
        /// created by:ntkien 20.05.2020
        public static object CreateBL(string entity)
        {
            try
            {
                //tìm trực tiếp đối tượng BL của entity
                string assemblyName = string.Format(AppSettingKey.ASSEMBLY_BL_NAME, entity);
                var types = AppDomain.CurrentDomain.Load(AppSettingKey.ASSEMBLY_BL);
                var type = types.DefinedTypes.FirstOrDefault(c => c.FullName.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
                var result = type != null ? Activator.CreateInstance(AppSettingKey.ASSEMBLY_BL_FULLNAME, type.FullName) : null;
                if (result==null)
                {
                    var obj = Factory.CreateEntity(entity);
                    Type typeobj = obj.GetType();
                    Type gType = typeof(BLBase<>).MakeGenericType(typeobj);
                    return Activator.CreateInstance(gType);
                }
                return result != null ? result.Unwrap() : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// khởi tạo đối tượng Entity theo tên 
        /// </summary>
        /// <param name="entity">tên của entity</param>
        /// <returns></returns>
        /// created by:ntkien 20.05.2020
        public static object CreateEntity(string entity)
        {
            object result = null;
            try
            {
                string assemblyName = string.Format(AppSettingKey.ASSEMBLY_ENTITY_NAME, entity);
                var typeobj = AppDomain.CurrentDomain.Load(AppSettingKey.ASSEMBLY_ENTITY);
                var type = typeobj.DefinedTypes.FirstOrDefault(c => c.FullName.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
                return Activator.CreateInstance(AppSettingKey.ASSEMBLY_ENTITY_FULLNAME, type.FullName).Unwrap();
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}
