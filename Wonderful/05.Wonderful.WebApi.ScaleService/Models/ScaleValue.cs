using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.ScaleService.Models
{
    /// <summary>
    /// 称重值
    /// </summary>
    [Table("TSTB_SCALE_VALUES")]
    public class ScaleValue
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ScaleValueId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }
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
