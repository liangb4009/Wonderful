using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.HautosProductService.Models.Requests
{
    /// <summary>
    /// 获取设备列表请求
    /// </summary>
    public class GetDeviceListRequest
    {
        /// <summary>
        /// 设备列表请求
        /// </summary>
        public GetDeviceListRequest()
        {
            this.page = 1;
            this.pageSize = 10;
        }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string deviceSerialNumber { get; set; }
        /// <summary>
        /// 设备名字
        /// </summary>
        public string deviceName { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string deviceType { get; set; }
        public List<int> deviceIds { get; set; }
        /// <summary>
        /// 设备Id
        /// </summary>
        public int eqid { get; set; }
        /// <summary>
        /// 设备列表页数
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 设备列表每页设备数
        /// </summary>
        public int pageSize { get; set; }
    }
}
