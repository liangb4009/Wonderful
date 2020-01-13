using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 拍照请求
    /// </summary>
    public class ImageCaptureRequest
    {
        /// <summary>
        /// 请求拍照的用户Id
        /// </summary>
        public string TokenId { get; set; }
    }
}
