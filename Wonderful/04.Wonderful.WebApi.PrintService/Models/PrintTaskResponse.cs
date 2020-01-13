using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.PrintService.Models
{
    /// <summary>
    /// 条码打印任务结果
    /// </summary>
    public class PrintTaskResponse
    {
        /// <summary>
        /// 打印任务Id
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 执行打印的结果，True表示成功，False表示失败
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 执行打印的结果消息，成功显示空字符串，失败显示原因
        /// </summary>
        public string Msg { get; set; }
    }
}
