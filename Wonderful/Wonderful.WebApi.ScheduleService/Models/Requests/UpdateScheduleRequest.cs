using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScheduleService.Models.Requests
{
    public class UpdateScheduleRequest
    {
        /// <summary>
        /// 任务id
        /// </summary>
        public string ScheduleId { get; set; }
        /// <summary>
        /// 任务code
        /// </summary>
        public string ScheduleCode { get; set; }
        /// <summary>
        /// 系统code
        /// </summary>
        public string SystemCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Descr { get; set; }
        /// <summary>
        /// 任务起始时间
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 执行周期
        /// </summary>
        public string Period { get; set; }
        /// <summary>
        /// 重试次数
        /// </summary>
        public string RetryCount { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string ScheduleStatus { get; set; }
    }
}
