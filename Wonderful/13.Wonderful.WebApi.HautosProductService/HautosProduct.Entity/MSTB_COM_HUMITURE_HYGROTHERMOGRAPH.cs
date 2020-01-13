using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.HautosProductService.HautosProduct.Entity
{
    /// <summary>
    /// 温湿度设备信息
    /// </summary>
    [Table("MSTB_COM_HUMITURE_HYGROTHERMOGRAPH")]
    public class MSTB_COM_HUMITURE_HYGROTHERMOGRAPH
    {
        /// <summary>
        /// 设备id
        /// </summary>
        [Key]
        public int EQUIPMENT_ID { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        [Column("DeviceArea")]
        public string DeviceArea { get; set; }

        /// <summary>
        /// 设备序列号
        /// </summary>
        [Column("DeviceSerialNumber")]
        public string DeviceSerialNumber { get; set; }
        /// <summary>
        /// 设备地址
        /// </summary>
        [Column("HygrothermographAddress")]
        public string HygrothermographAddress { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [Column("HygrothermographName")]
        public string HygrothermographName { get; set; }
        /// <summary>
        /// 设备端口
        /// </summary>
        [Column("HygrothermographPort")]
        public int HygrothermographPort { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        [Column("HygrothermographType")]
        public HygrothermographType HygrothermographType { get; set; }
        /// <summary>
        /// 采集频率
        /// </summary>
        [Column("GetRate")]
        public int? GetRate { get; set; }
        /// <summary>
        /// 温度上限
        /// </summary>
        [Column("TempratureUper")]
        public decimal? TempratureUper { get; set; }
        /// <summary>
        /// 温度下限
        /// </summary>
        [Column("TempratureLower")]
        public decimal? TempratureLower { get; set; }
        /// <summary>
        ///湿度上限
        /// </summary>
        [Column("HumidityUper")]
        public decimal? HumidityUper { get; set; }
        /// <summary>
        /// 湿度下限
        /// </summary>
        [Column("HumidityLower")]
        public decimal? HumidityLower { get; set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        [Column("ConnectStatus")]
        public string ConnectStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("Status")]
        public string Status { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("UpdateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Column("RecordStatus")]
        public int? RecordStatus { get; set; }
        /// <summary>
        /// 记录更新时间
        /// </summary>
        [Column("RecordLastEditDt")]
        public DateTime? RecordLastEditDt { get; set; }
        [Column("RecordGuid")]
        public Guid? RecordGuid { get; set; }

        public virtual ICollection<TSTB_QUA_HUMITURE_DATA> TSTB_QUA_HUMITURE_DATAS { get; set; }
    }
}
