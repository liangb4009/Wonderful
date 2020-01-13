using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 保存称重任务响应
    /// </summary>
    public class ScaleSaveTaskResponse
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 保存称重任务结果消息，成功显示空字符串，失败显示原因
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 保存称重任务的结果，True表示成功，False表示失败
        /// </summary>
        public bool Result { get; set; }
    }
}
