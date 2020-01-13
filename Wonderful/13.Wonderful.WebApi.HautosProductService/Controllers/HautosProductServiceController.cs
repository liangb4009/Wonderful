using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Wonderful.WebApi.HautosProductService.HautoContext;
using Wonderful.WebApi.HautosProductService.HautosProduct.Entity;
using Wonderful.WebApi.HautosProductService.Models;
using Wonderful.WebApi.HautosProductService.Models.Requests;

namespace _13.Wonderful.WebApi.HautosProductService.Controllers
{
    /// <summary>
    /// 温湿度计服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HautosProductServiceController : ControllerBase
    {
        private readonly HautoProductSqlContext _hautoProductContext;


        public HautosProductServiceController(HautoProductSqlContext hautoProductContext)
        {
            this._hautoProductContext = hautoProductContext;
        }


        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="request">获取设备列表请求</param>
        /// <returns>华图温湿度设备</returns>
        [HttpPost("GetHautoDeviceList")]
        public ActionResult GetHautoDeviceList(GetDeviceListRequest request)
        {

            var modelList = new List<MSTB_COM_HUMITURE_HYGROTHERMOGRAPHModel>();
            var q = _hautoProductContext.MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS as IQueryable<MSTB_COM_HUMITURE_HYGROTHERMOGRAPH>;


            if (!request.deviceSerialNumber.IsNullOrEmpty())
            {
                q = q.Where(p => p.DeviceSerialNumber == request.deviceSerialNumber);
            }
            if (!request.deviceName.IsNullOrEmpty())
            {
                q = q.Where(p => p.HygrothermographName.Contains(request.deviceName));
            }
            if (!request.deviceType.IsNullOrEmpty())
            {
                var deviceType = (HygrothermographType)(int.Parse(request.deviceType));
                q = q.Where(p => p.HygrothermographType == deviceType);
            }
            if (request.deviceIds != null && request.deviceIds.Count > 0)
            {
                q = q.Where(p => request.deviceIds.Contains(p.EQUIPMENT_ID));
            }
       
            var rowCount = q.Count();

            q = q.Skip((request.page - 1) * request.pageSize).Take(request.pageSize);

            var dataList = q.ToList();

            if (dataList != null && dataList.Count > 0)
            {
                foreach (var item in dataList)
                {
                    var tmpModel = new MSTB_COM_HUMITURE_HYGROTHERMOGRAPHModel
                    {

                        DeviceArea = item.DeviceArea,
                        DeviceSerialNumber = item.DeviceSerialNumber,
                        EQUIPMENT_ID = item.EQUIPMENT_ID,
                        GetRate = item.GetRate.HasValue ? item.GetRate.Value : 10,
                        TempratureUper =item.TempratureUper,
                        TempratureLower =item.TempratureLower,
                        HumidityUper = item.HumidityUper,
                        HumidityLower = item.HumidityLower,
                        HygrothermographAddress = item.HygrothermographAddress,
                        HygrothermographName = item.HygrothermographName,
                        HygrothermographPort = item.HygrothermographPort,
                        HygrothermographType = item.HygrothermographType.ToString(),
                        HumidityUperLowerStr=(item.HumidityUper.HasValue&&item.HumidityLower.HasValue)?(item.HumidityLower.Value.ToString()+" - "+item.HumidityUper.ToString()) :"",
                        TempratureUpLowerStr= (item.TempratureUper.HasValue && item.TempratureLower.HasValue) ? (item.TempratureLower.Value.ToString() + " - " + item.TempratureUper.ToString()) : "",
                        Status = item.Status,
                        ConnectStatusValue=item.ConnectStatus
                    };
                    if (item.ConnectStatus==EnumConnectStatus.Connected.ToString())
                    {
                        tmpModel.ConnectStatus = "连接";
                    }
                    else if(item.ConnectStatus == EnumConnectStatus.Disconnected.ToString())
                    {
                        tmpModel.ConnectStatus = "断开";
                    }
                    modelList.Add(tmpModel);
                }
            }


            return new JsonResult(new { Datas = modelList, page = request.page, pageSize = request.pageSize, recordCount = rowCount, result = true });

        }
        /// <summary>
        /// 根据设备id获取设备信息
        /// </summary>
        /// <param name="eqid">设备id</param>
        /// <returns>找到返回True，华图温湿度设备信息；没找到返回False，错误信息“未找到相关设备！”</returns>      
        [HttpPost("GetDeviceDetail")]        
        public ActionResult GetDeviceDetail(GetDeviceListRequest request)
        {
            var model = default(MSTB_COM_HUMITURE_HYGROTHERMOGRAPHModel);
            if (request.eqid > 0)
            {
                var deviceInfo = _hautoProductContext.MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS.FirstOrDefault(p=>p.EQUIPMENT_ID== request.eqid);
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
                    return new JsonResult(new { Success=false,ErrorMsg="未找到相关设备！"});
                }
            }
            return new JsonResult(new { Success=true,data=model});
        }
        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="deviceInfo">华图温湿度设备</param>
        /// <returns>保存成功，返回Success为True，ErrorMsg为空；保存失败，返回Success为True，ErrorMsg为空，ErrorDetailMsg返回错误信息</returns>
        [HttpPost("SaveHautoDevice")]
        public ActionResult SaveHautoDevice(MSTB_COM_HUMITURE_HYGROTHERMOGRAPHModel deviceInfo)
        {
            StringBuilder errorMsg = new StringBuilder();
            StringBuilder errorExceptionMsg = new StringBuilder();
            if (deviceInfo.EQUIPMENT_ID == 0)
            {

            }
            else
            {

                var updateDevice = _hautoProductContext.MSTB_COM_HUMITURE_HYGROTHERMOGRAPHS.FirstOrDefault(p => p.EQUIPMENT_ID == deviceInfo.EQUIPMENT_ID);
                if (!deviceInfo.ConnectStatusValue.IsNullOrEmpty())
                {
                    updateDevice.ConnectStatus = deviceInfo.ConnectStatusValue.ToString();
                }
                if (!deviceInfo.DeviceArea.IsNullOrEmpty())
                {
                    updateDevice.DeviceArea = deviceInfo.DeviceArea;
                }
                if (!deviceInfo.HygrothermographAddress.IsNullOrEmpty())
                {
                    updateDevice.HygrothermographAddress = deviceInfo.HygrothermographAddress;
                }
                try
                {                  
                    _hautoProductContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    errorExceptionMsg.Append(ex.Message.ToString() + ex.InnerException.ToString());
                }
            }

            return new JsonResult(new { Success = true, ErrorMsg = "", ErrorDetailMsg = errorMsg.ToString() });
        }

        /// <summary>
        /// 批量记录温湿度数据
        /// </summary>
        /// <param name="dataList">温湿度数据</param>
        /// <returns>保存成功，返回Success为True</returns>
        [HttpPost("LogTemperatureAndHumidityList")]
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
        /// <summary>
        /// 获取温湿度数据记录
        /// </summary>
        /// <param name="begintime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns>温湿度数据列表，Success为True</returns>
        [HttpPost("GetTemperatureAndHumidityList")]
        public ActionResult GetTemperatureAndHumidityList(string begintime, string endtime)
        {
            var modelList = new List<TSTB_QUA_HUMITURE_DATAModel>();

            var q = _hautoProductContext.TSTB_QUA_HUMITURE_DATAS as IQueryable<TSTB_QUA_HUMITURE_DATA>;

            if (!string.IsNullOrEmpty(begintime) && !string.IsNullOrEmpty(endtime))
            {
                var beginDatetime = DateTime.Parse(begintime);
                var endDatetime = DateTime.Parse(endtime);
                q = q.Where(p => p.DATE_TIME > beginDatetime && p.DATE_TIME < endDatetime);
                var dataList = q.Where(p => p.DATE_TIME > beginDatetime && p.DATE_TIME < endDatetime).ToList();
            }

            return new JsonResult(new { Datas = modelList, Success = true });
        }
    }
}