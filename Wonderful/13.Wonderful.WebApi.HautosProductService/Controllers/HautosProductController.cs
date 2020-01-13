using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Wonderful.WebApi.HautosProductService.HautoContext;
using Wonderful.WebApi.HautosProductService.HautosProduct.Entity;
using Wonderful.WebApi.HautosProductService.Models;
using Wonderful.WebApi.HautosProductService.Models.Requests;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wonderful.WebApi.HautosProductService.Controllers
{
    public class HautosProductController : Controller
    {
        // GET: /<controller>/
        private readonly HautoProductSqlContext _hautoProductContext;

        public HautosProductController(HautoProductSqlContext hautoProductContext)
        {
            this._hautoProductContext = hautoProductContext;
        }

        // GET: /<controller>/
        public ActionResult HautoProductIndex()
        {
            //var device1 = new MSTB_COM_HUMITURE_HYGROTHERMOGRAPH
            //{
            //    DeviceSerialNumber = "HS500BS713",               
            //    HygrothermographAddress = "192.168.1.90",
            //    HygrothermographName = "NSG-T/H42",
            //    HygrothermographPort = 4001,
            //    HygrothermographType = HygrothermographType.RJ45,
            //    Status = "6#干燥间",
            //    UpdateTime = DateTime.Now
            //};
            //if (_hautoProductContext.MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS.FirstOrDefault(p => p.DeviceSerialNumber == device1.DeviceSerialNumber) == null)
            //{
            //    _hautoProductContext.MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS.Add(device1);
            //    _hautoProductContext.SaveChanges();
            //}
            //var deviceInfo = _hautoProductContext.MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS.FirstOrDefault();
            return View();
        }
        /// <summary>
        /// 批量记录温湿度数据
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogTemperatureAndHumidityList(List<TSTB_QUA_HUMITURE_DATAModel> modelList)
        {
            var dataList = new List<TSTB_QUA_HUMITURE_DATA>();
            foreach (var item in modelList)
            {
                var tmpData = new TSTB_QUA_HUMITURE_DATA
                {
                    DATE_TIME = DateTime.Parse(item.DATE_TIME),
                    EQUIPMENT_ID = item.EQUIPMENT_ID,
                    HUMIDITY = item.HUMIDITY,
                    IsSendMail = 0,
                    RecordGuid = Guid.NewGuid(),
                    RecordLastEditDt = DateTime.Now,
                    RecordStatus = 0,
                    RoomInfo = item.RoomInfo,
                    TEMPERATURE = item.TEMPERATURE
                };
                dataList.Add(tmpData);
            }
            _hautoProductContext.TSTB_QUA_HUMITURE_DATAS.AddRange(dataList);
            _hautoProductContext.SaveChanges();
            return new JsonResult(new { Success = true });
        }
        [HttpPost]
        public ActionResult GetDeviceDetail(int eqid)
        {
            var model = default(MSTB_COM_HUMITURE_HYGROTHERMOGRAPHModel);
            if (eqid > 0)
            {
                var deviceInfo = _hautoProductContext.MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS.FirstOrDefault(p => p.EQUIPMENT_ID == eqid);
                if (deviceInfo != null)
                {
                    model = new MSTB_COM_HUMITURE_HYGROTHERMOGRAPHModel
                    {
                        DeviceArea = deviceInfo.DeviceArea,
                        DeviceSerialNumber = deviceInfo.DeviceSerialNumber,
                        EQUIPMENT_ID = deviceInfo.EQUIPMENT_ID,
                        GetRate = deviceInfo.GetRate.HasValue ? deviceInfo.GetRate.Value : 10,
                        TempratureUper = deviceInfo.TempratureUper,
                        TempratureLower = deviceInfo.TempratureLower,
                        HumidityUper = deviceInfo.HumidityUper,
                        HumidityLower = deviceInfo.HumidityLower,
                        HygrothermographAddress = deviceInfo.HygrothermographAddress,
                        HygrothermographName = deviceInfo.HygrothermographName,
                        HygrothermographPort = deviceInfo.HygrothermographPort,
                        HygrothermographType = deviceInfo.HygrothermographType.ToString(),
                        HumidityUperLowerStr = (deviceInfo.HumidityUper.HasValue && deviceInfo.HumidityLower.HasValue) ? (deviceInfo.HumidityLower.Value.ToString() + " - " + deviceInfo.HumidityUper.ToString()) : "",
                        TempratureUpLowerStr = (deviceInfo.TempratureUper.HasValue && deviceInfo.TempratureLower.HasValue) ? (deviceInfo.TempratureLower.Value.ToString() + " - " + deviceInfo.TempratureUper.ToString()) : "",
                        Status = deviceInfo.Status,
                        ConnectStatusValue = deviceInfo.ConnectStatus
                    };
                    if (deviceInfo.ConnectStatus == EnumConnectStatus.Connected.ToString())
                    {
                        model.ConnectStatus = "连接";
                    }
                    else if (deviceInfo.ConnectStatus == EnumConnectStatus.Disconnected.ToString())
                    {
                        model.ConnectStatus = "断开";
                    }
                }
                else
                {
                    return new JsonResult(new { Success = false, ErrorMsg = "未找到相关设备！" });
                }
            }
            return new JsonResult(new { Success = true, data = model });
        }

    }
}
