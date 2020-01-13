using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ImageService.Models
{
    /// <summary>
    /// 图像值
    /// </summary>
    [Table("TSTB_IMAGE_VALUES")]
    public class ImageValue
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// TaskId
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// TodoItem
        /// </summary>
        public TodoItem TodoItem { get; set; }
    }
}
