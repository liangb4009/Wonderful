using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.PrintService.Models
{
    /// <summary>
    /// 条码打印任务
    /// </summary>
    public class PrintTaskRequest
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

        /// <summary>
        /// 打印任务需要替换字符串
        /// </summary>
        public IEnumerable<NamedSubString> TaskContent { get; set; }

        /// <summary>
        /// Excel打印任务需要的内容
        /// </summary>
        public IEnumerable<ExcelTaskContent> ExcelTaskContent { get; set; }

        /// <summary>
        /// 是否打印下一个标签，True-是，False-否
        /// 如果需要打印下一个标签，不关闭预加载模板
        /// 如果不需要打印下一个标签，关闭预加载模板
        /// </summary>
        public bool PrintNext{get;set;}
    }
}
