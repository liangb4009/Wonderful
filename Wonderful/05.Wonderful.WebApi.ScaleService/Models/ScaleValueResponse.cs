using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 称重值响应
    /// </summary>
    public class ScaleValueResponse
    {
        /// <summary>
        /// 称重值Id
        /// </summary>
        public int ScaleValueId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
    }
}
