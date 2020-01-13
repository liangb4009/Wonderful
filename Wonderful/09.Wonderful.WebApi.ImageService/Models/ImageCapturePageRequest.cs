using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 图像拍照页面请求
    /// </summary>
    public class ImageCapturePageRequest
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 运行模型，分拍照(takephoto)，扫描条码(scanqrcode)
        /// </summary>
        public string RunMode { get; set; }
        /// <summary>
        /// 摄像头类型，分前置(front camera)，后置(back camera)
        /// </summary>
        public string CameraType { get; set; }
        /// <summary>
        /// 安装摄像头机器名
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// 拍照次数
        /// </summary>
        public int CaptureCount { get; set; }
        /// <summary>
        /// 拍照频率，单位秒
        /// </summary>
        public int CaptureFrequency { get; set; }
        /// <summary>
        /// 图像值
        /// </summary>
        public IEnumerable<ImageValueRequest> ImageValues { get; set; }
    }
}
