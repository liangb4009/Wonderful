using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 拍照响应
    /// </summary>
    public class ImageCaptureResponse
    {
        /// <summary>
        /// 请求拍照用户的TokenId
        /// </summary>
        public string TokenId { get; set; }
        /// <summary>
        /// 请求拍照的结果，True表示成功，False表示失败
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 请求拍照的结果消息，成功显示空字符串，失败显示原因
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 拍照任务Id
        /// </summary>
        public string TaskId { get; set; }
    }
}
