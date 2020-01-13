using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.HautosProductService.HautosProduct.Entity
{
    /// <summary>
    /// 温湿度数据
    /// </summary>
    [Table("TSTB_QUA_HUMITURE_DATA")]
    public class TSTB_QUA_HUMITURE_DATA
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        [Column("EQUIPMENT_ID")]
        public int EQUIPMENT_ID { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        [Column("DATE_TIME")]
        public DateTime DATE_TIME { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        [Column("TEMPERATURE")]
        public decimal TEMPERATURE { get; set; }
        /// <summary>
        /// 湿度
        /// </summary>
        [Column("HUMIDITY")]
        public decimal HUMIDITY { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Column("RecordStatus")]
        public int RecordStatus { get; set; }
        [Column("RecordLastEditDt")]
        public DateTime? RecordLastEditDt { get; set; }
        [Column("RecordGuid")]
        public Guid? RecordGuid { get; set; }
        /// <summary>
        /// 房间信息
        /// </summary>
        [Column("RoomInfo")]
        public string RoomInfo { get; set; }
        [Column("IsSendMail")]
        public int? IsSendMail { get; set; }

        public virtual MSTB_COM_HUMITURE_HYGROTHERMOGRAPH MSTBCOMHUMITUREHYGROTHERMOGRAPH { get; set; }
    }
}
