using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 电子秤参数
    /// </summary>
    [Table("TSTB_SCALE_PARAMS")]
    public class ScaleParams
    {
        /// <summary>
        /// 电子秤参数id
        /// </summary>
        public int ScaleParamId { get; set; }
        /// <summary>
        /// 电子秤名，外键
        /// </summary>
        public string ScaleName { get; set; }
        /// <summary>
        /// 电子秤端口
        /// </summary>
        public string ScalePort { get; set; }
        /// <summary>
        /// 电子秤比特率
        /// </summary>
        public int ScaleBitrate { get; set; }

        public string TaskId { get; set; }
        /// <summary>
        /// 代办项
        /// </summary>
        public TodoItem todoItem { get; set; }
    }
}
