using NorthWind.Library;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Entity Factory
/// </summary>
/// created by: ntkien 21.05.2020
namespace NorthWind.BL
{
    public static class EntityFactory
    {
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
                var type = typeobj.DefinedTypes.FirstOrDefault(c => c.FullName.StartsWith(assemblyName, StringComparison.OrdinalIgnoreCase));
                return Activator.CreateInstance(AppSettingKey.ASSEMBLY_ENTITY_FULLNAME, type.FullName).Unwrap();
            }
            catch (Exception ex)
            {
                return result;
            }
        }
    }
}
