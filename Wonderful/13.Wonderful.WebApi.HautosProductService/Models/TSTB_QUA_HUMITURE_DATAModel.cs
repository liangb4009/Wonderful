using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.HautosProductService.Models
{
    /// <summary>
    /// 温湿度数据
    /// </summary>
    public class TSTB_QUA_HUMITURE_DATAModel
    {
        /// <summary>
        /// 记录唯一ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        public int EQUIPMENT_ID { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public string DATE_TIME { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public decimal TEMPERATURE { get; set; }
        /// <summary>
        /// 湿度
        /// </summary>
        public decimal HUMIDITY { get; set; }
        /// <summary>
        /// 房间信息
        /// </summary>
        public string RoomInfo { get; set; }
    }
}
