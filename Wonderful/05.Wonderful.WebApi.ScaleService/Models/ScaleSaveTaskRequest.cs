using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 保存称重任务请求
    /// </summary>
    public class ScaleSaveTaskRequest
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 称重任务类型，分自动-auto，手动-manual
        /// </summary>
        public string ScaleType { get; set; }
        /// <summary>
        /// 电子秤名
        /// </summary>
        public string ScaleName { get; set; }
        /// <summary>
        /// 称重次数
        /// </summary>
        public int ScaleCount { get; set; }
        /// <summary>
        /// 称重频率
        /// </summary>
        public int ScaleFrequency { get; set; }
        /// <summary>
        /// 称参数，包括端口、波特率等
        /// </summary>
        public ScaleParamsRequest ScaleParams { get; set; }
    }
}
