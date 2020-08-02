using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NorthWind.Library;
using NorthWind.Entity;
using NorthWind.BL;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;
using NorthWind.DL;
using Microsoft.EntityFrameworkCore;

namespace NorthWind.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            BLBase<Product> bl = new BLBase<Product>();

            return "service OK";
        }
        [HttpGet, Route("getkey/{key}")]
        public async Task<ServiceResult> GetKey(string key)
        {
            ServiceResult service = new ServiceResult();
            List<Customer> datas = null;
            using (NorthWindContext<Customer> context = new NorthWindContext<Customer>())
            {
                datas = await context.ListBase.ToListAsync();
            }
            service.Data = JsonConvert.SerializeObject(datas);
            return service;
        }
        [HttpGet, Route("getkeyl/{key}")]
        public async Task<string> GetKeyL(string key)
        {
            ServiceResult service = new ServiceResult();
            service.Data = key;
            return key;
        }
        //[HttpGet,Route("getkey/{key}")]
        //public string GetKey(string key)
        //{
        //    string result = string.Empty;
        //    BLBase<Customer> bl = new BLBase<Customer>();
        //    Customer obj = new Customer();
        //    obj.CustomerId =Guid.Parse("D796CB8F-91CE-4606-B65B-043BAA3018CB");
        //    obj.CustomerCode = "Code7";
        //    obj.CustomerName = "Name7";
        //    obj.Address = "Address3";
        //    obj.Phone = "Phone3";
        //    obj.EditMode = Enumeration.EditMode.Edit;
        //    obj.CreatedDate = DateTime.Now;
        //    obj.ModifiedDate = DateTime.Now;


        //    Customer obj1 = new Customer();
        //    obj1.CustomerId = Guid.Parse("F48979AD-CF41-4EAB-ABFA-B33DA64C13E2");
        //    obj1.CustomerCode = "Code4";
        //    obj1.CustomerName = "Name4";
        //    obj1.Address = "Address4";
        //    obj1.Phone = "Phone4";
        //    obj1.EditMode = Enumeration.EditMode.Delete;
        //    obj1.CreatedDate = DateTime.Now;
        //    obj1.ModifiedDate = DateTime.Now;

        //    bl.UpdateData.Add(obj);
        //    bl.UpdateData.Add(obj1);
        //    bl.SaveData();
        //    return result;
        //}


        //[HttpGet, Route("setail/{key}")]
        //public string setail(string key)
        //{
        //    string result = string.Empty;
        //    BLBase<Order> bl = new BLBase<Order>();
        //    Order order = new Order();
        //    order.OrderId = Guid.NewGuid();
        //    order.OrderDate = DateTime.Now;
        //    order.PaymentType = 1;
        //    order.IsPaid = true;
        //    order.Amount = 100;
        //    order.TotalAmount = 100;
        //    order.Vatrate = 100;
        //    order.Vatamount = 100;
        //    order.EditMode = Enumeration.EditMode.Add;
        //    order.CreatedDate = DateTime.Now;
        //    order.ModifiedDate = DateTime.Now;

        //    OrderDetail detail = new OrderDetail();
        //    detail.OrderDetailId = Guid.NewGuid();
        //    detail.OrderId = order.OrderId;
        //    detail.ProductId = Guid.Parse("F48979AD-CF41-4EAB-ABFA-B33DA64C13E2");
        //    detail.Quantity = 1;
        //    detail.UnitPrice = 20000;
        //    detail.Amount = 20000;
        //    detail.EditMode = Enumeration.EditMode.Add;
        //    detail.CreatedDate = DateTime.Now;
        //    detail.ModifiedDate = DateTime.Now;

        //    OrderDetail detail2 = new OrderDetail();
        //    detail2.OrderDetailId = Guid.NewGuid();
        //    detail2.OrderId = order.OrderId;
        //    detail2.ProductId = Guid.Parse("51E59BF3-50F5-47B6-A5B7-79DDDFDE2B85");
        //    detail2.Quantity = 3;
        //    detail2.UnitPrice = 10000;
        //    detail2.Amount = 30000;
        //    detail2.EditMode = Enumeration.EditMode.Add;
        //    detail2.CreatedDate = DateTime.Now;
        //    detail2.ModifiedDate = DateTime.Now;

        //    List<OrderDetail> d = new List<OrderDetail>() { detail, detail2 };

        //    bl.UpdateData.Add(order);
        //    bl.DetailData = new List<DetailObject>();
        //    bl.DetailData.Add( new DetailObject() { TableName = detail.GetType().Name, Value = JsonConvert.SerializeObject(detail) });
        //    bl.DetailData.Add(new DetailObject() { TableName = detail.GetType().Name, Value = JsonConvert.SerializeObject(detail2) });
        //    bl.SaveData();
        //    return result;
        //}

        //[HttpGet, Route("get2")]
        //public string get2(string key)
        //{
        //    PagingObject pagingObject = new PagingObject();
        //    List<OrderByObject> orderbys = new List<OrderByObject>();
        //    orderbys.Add(new OrderByObject() { ColumnName = "ProductCode" });
        //    List<WhereObject> wheres = new List<WhereObject>();
        //    wheres.Add(new WhereObject() { ColumnName = "ProductCode",Value="BARA" });
        //    pagingObject.OrderInfos = orderbys;
        //    pagingObject.WhereInfos = wheres;
        //    return JsonConvert.SerializeObject(pagingObject);
        //}
    }
}
