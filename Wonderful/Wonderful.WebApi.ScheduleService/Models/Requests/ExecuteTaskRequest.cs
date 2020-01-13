using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScheduleService.Models.Requests
{
    public class ExecuteTaskRequest
    {
        /// <summary>
        /// 任务code
        /// </summary>
        public string ScheduleCode { get; set; }
        /// <summary>
        /// 系统code
        /// </summary>
        public string SystemCode { get; set; }
    }
}
