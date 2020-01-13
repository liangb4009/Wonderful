using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.HautosProductService.Models
{
    /// <summary>
    /// 华图温湿度设备
    /// </summary>
    public class MSTB_COM_HUMITURE_HYGROTHERMOGRAPHModel
    {
        /// <summary>
        /// 区域
        /// </summary>
        public string DeviceArea { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public int EQUIPMENT_ID { get; set; }
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DeviceSerialNumber { get; set; }
        /// <summary>
        /// 设备地址
        /// </summary>
        public string HygrothermographAddress { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string HygrothermographName { get; set; }
        /// <summary>
        /// 设备端口
        /// </summary>
        public int? HygrothermographPort { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string HygrothermographType { get; set; }
        /// <summary>
        /// 采集频率
        /// </summary>
        public int? GetRate { get; set; }
        /// <summary>
        /// 温度上限
        /// </summary>
        public decimal?  TempratureUper { get; set; }
        /// <summary>
        /// 温度下限
        /// </summary>
        public decimal? TempratureLower { get; set; }
        /// <summary>
        /// 温度上下限描述
        /// </summary>
        public string  TempratureUpLowerStr { get; set; }

        /// <summary>
        /// 湿度上限
        /// </summary>
        public decimal? HumidityUper { get; set; }

        /// <summary>
        /// 湿度下限
        /// </summary>
        public decimal? HumidityLower { get; set; }
        /// <summary>
        /// 湿度上下限描述
        /// </summary>
        public string HumidityUperLowerStr { get; set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        public string ConnectStatus { get; set; }
        /// <summary>
        /// 连接状态value
        /// </summary>
        public string ConnectStatusValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Status { get; set; }
    }
}
