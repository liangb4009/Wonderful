using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 待办请求响应
    /// </summary>
    public class TodoItemResponse
    {
        /// <summary>
        /// TokenId
        /// </summary>
        public string TokenId { get; set; }
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 称重类型，分manual(手工)、auto(自动)
        /// </summary>
        public string ScaleType { get; set; }
        /// <summary>
        /// 称名字
        /// </summary>
        public string ScaleName { get; set; }
        /// <summary>
        /// 称多少次
        /// </summary>
        public int ScaleCount { get; set; }
        /// <summary>
        /// 称频率，当ScantType=auto时有效
        /// </summary>
        public int ScaleFrequency { get; set; }
        /// <summary>
        /// 是否完成，True-完成，False-未完成
        /// </summary>
        public bool IsComplete { get; set; }
        /// <summary>
        /// 称重参数
        /// </summary>
        public ScaleParamsResponse ScaleParams { get; set; }
        /// <summary>
        /// 称重值
        /// </summary>
        public List<ScaleValueResponse> ScaleValues { get; set; }
    }
}
