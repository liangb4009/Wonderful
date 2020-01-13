using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 请求称重页面
    /// </summary>
    public class ScalePageRequest
    {
        /// <summary>
        /// 称重任务Id
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 称重任务电子称名
        /// </summary>
        public string ScaleName { get; set; }
        /// <summary>
        /// 称重方式，包括手工(manual)，自动(auto)
        /// </summary>
        public string ScaleType { get; set; }
        /// <summary>
        /// 电子秤参数
        /// </summary>
        public ScaleParams ScaleParams { get; set; }
        /// <summary>
        /// 称重次数
        /// </summary>
        public int ScaleCount { get; set; }
        /// <summary>
        /// 称重频率，单位秒
        /// </summary>
        public int ScaleFrequency { get; set; }
        /// <summary>
        /// 称量值
        /// </summary>
        public IEnumerable<ScaleValueRequest> ScaleValues { get; set; }
    }
}
