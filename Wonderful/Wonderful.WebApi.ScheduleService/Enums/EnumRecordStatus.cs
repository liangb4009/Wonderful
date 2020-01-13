using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScheduleService.Enums
{
    public enum EnumRecordStatus
    {
        /// <summary>
        /// 无效
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// 有效
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 9
    }
}
