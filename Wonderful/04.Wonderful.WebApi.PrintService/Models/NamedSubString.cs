using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.PrintService.Models
{
    /// <summary>
    /// 替代字符串
    /// </summary>
    public class NamedSubString
    {
        /// <summary>
        /// 替代字符串名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 替代字符串值
        /// </summary>
        public string Value { get; set; }
    }
}
