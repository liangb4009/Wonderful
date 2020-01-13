﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 保存拍照任务请求
    /// </summary>
    public class ImageSaveCaptureTaskRequest
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 运行模型，分拍照(takephoto)、扫描条码(scanqrcode)
        /// </summary>
        public string RunMode { get; set; }
        /// <summary>
        /// 摄像头类型，分前置(front camera)、后置(back camera)
        /// </summary>
        public string CameraType { get; set; }
        /// <summary>
        /// 安装摄像头的电脑名字
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// 拍照次数
        /// </summary>
        public int CaptureCount { get; set; }
        /// <summary>
        /// 拍照频率
        /// </summary>
        public int CaptureFrequency { get; set; }
    }
}
