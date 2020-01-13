using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 请求保存当前重量结果
    /// </summary>
    public class ScaleSaveValueResponse
    {
        /// <summary>
        /// 称重TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 请求称重的结果消息，成功显示"Success to save value"，失败显示原因
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 请求称重的结果，True表示成功，False表示失败
        /// </summary>
        public bool Result { get; set; }
    }
}
