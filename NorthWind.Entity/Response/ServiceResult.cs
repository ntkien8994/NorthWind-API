using System;
using System.Collections.Generic;
using System.Text;
/// <summary>
/// class chứa đối tượng trả về cho object
/// </summary>
namespace NorthWind.Entity
{
    public class ServiceResult
    {
        public string Message { get; set; }
        public int ResultCode { get; set; }
        public string Data { get; set; }
        public bool Success { get; set; } = true;
    }
}
