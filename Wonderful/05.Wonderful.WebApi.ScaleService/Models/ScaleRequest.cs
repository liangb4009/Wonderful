using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 请求称重
    /// </summary>
    public class ScaleRequest
    {
        /// <summary>
        /// 请求称重的用户Id
        /// </summary>
        public string TokenId { get; set; }
    }
}
