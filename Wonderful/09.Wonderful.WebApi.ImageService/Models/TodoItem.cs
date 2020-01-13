using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 待处理项目
    /// </summary>
    [Table("TSTB_TODOITEM_IMAGE")]
    public class TodoItem
    {
        /// <summary>
        /// TokenId
        /// </summary>
        public string TokenId { get; set; }

        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 运行模式，拍照(takephoto)、扫描二维码(scanqrcode)
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
        /// 拍照频率，单位秒
        /// </summary>
        public int CaptureFrequency { get; set; }

        /// <summary>
        /// 图像值
        /// </summary>
        public List<ImageValue> ImageValues { get; set; }

        /// <summary>
        /// 是否完成，True-完成，False-未完成
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
    }
}
