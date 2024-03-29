﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    public class ScaleParamsResponse
    {
        /// <summary>
        /// 电子秤参数id
        /// </summary>
        public int ScaleParamId { get; set; }
        /// <summary>
        /// 电子秤名
        /// </summary>
        public string ScaleName { get; set; }
        /// <summary>
        /// 电子秤端口
        /// </summary>
        public string ScalePort { get; set; }
        /// <summary>
        /// 电子秤比特率
        /// </summary>
        public int ScaleBitrate { get; set; }
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
    }
}
