using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 保存拍照任务响应
    /// </summary>
    public class ImageSaveCaptureTaskResponse
    {
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 保存拍照任务响应，成功显示空字符串，失败显示原因
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 保存拍照任务结果，True表示成功，False表示失败
        /// </summary>
        public bool Result { get; set; }
    }
}
