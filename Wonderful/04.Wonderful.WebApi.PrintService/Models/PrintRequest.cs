using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.PrintService.Models
{
    /// <summary>
    /// 请求打印
    /// </summary>
    public class PrintRequest
    {
        /// <summary>
        /// 请求打印的用户的TokenId
        /// </summary>
        public string TokenId { get; set; }
    }
}
