using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.PrintService.Models
{
    /// <summary>
    /// 执行打印
    /// </summary>
    public class ExecutePrintRequest
    {
        /// <summary>
        /// 打印任务Id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 打印文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 打印机名
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 打印类型（Bartend、Excel）
        /// </summary>
        public string PrintType { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintCount { get; set; }
    }
}
