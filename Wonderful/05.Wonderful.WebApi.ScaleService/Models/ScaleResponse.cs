using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 请求称重结果
    /// </summary>
    public class ScaleResponse
    {
        /// <summary>
        /// 请求称重用户的TokenId
        /// </summary>
        public string TokenId { get; set; }
        /// <summary>
        /// 请求称重的结果，True表示成功，False表示失败
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 请求称重的结果消息，成功显示空字符串，失败显示原因
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 称重任务Id
        /// </summary>
        public string TaskId { get; set; }
    }
}
