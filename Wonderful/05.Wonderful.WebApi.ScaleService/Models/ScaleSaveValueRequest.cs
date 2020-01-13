using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 请求保存当前重量
    /// </summary>
    public class ScaleSaveValueRequest
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 称量值
        /// </summary>
        public ScaleValueRequest ScaleValue { get; set; }
    }
}
