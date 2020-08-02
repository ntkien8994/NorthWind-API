using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Library
{
    /// <summary>
    /// class dùng để chứa tất cả các key 
    /// </summary>
    /// created by: ntkien 11.05.2020
    public static class AppSettingKey
    {
        //connection string
        public const string CONNECTION_STRING = "ConnectionStrings:DefaultConnectionString";

        //Assembly
        public const string ASSEMBLY_BL = "NorthWind.BL";
        public const string ASSEMBLY_BL_FULLNAME = "NorthWind.BL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        public const string ASSEMBLY_BL_NAME = "NorthWind.BL.BL{0}";

        public const string ASSEMBLY_ENTITY = "NorthWind.Entity";
        public const string ASSEMBLY_ENTITY_FULLNAME = "NorthWind.Entity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        public const string ASSEMBLY_ENTITY_NAME = "NorthWind.Entity.{0}";

    }
}
