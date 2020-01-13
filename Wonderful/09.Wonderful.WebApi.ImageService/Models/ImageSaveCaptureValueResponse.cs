using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 保存当前摄像头图像响应
    /// </summary>
    public class ImageSaveCaptureValueResponse
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 保存图像结果消息，成功显示"Success to save value"，失败显示原因
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 保存图像结果，True表示成功，False表示失败
        /// </summary>
        public bool Result { get; set; }
    }
}
