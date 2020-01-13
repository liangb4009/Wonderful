using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 保存当前摄像头图像请求
    /// </summary>
    public class ImageSaveCaptureValueRequest
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 图像值
        /// </summary>
        public List<ImageValueRequest> ImageValue { get; set; }
    }
}
