using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wonderful.WebApi.PrintService.Models
{
    /// <summary>
    /// 待处理项目
    /// </summary>
    [Table("TSTB_TODOITEM_PRINT")]
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
        /// 模板文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 打印机名
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 打印类型（Bartend、Excel）
        /// </summary>
        public string PrintType { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintCount { get; set; }

        /// <summary>
        /// 当前路径
        /// </summary>
        public string CurrentDirectory { get; set; }

        /// <summary>
        /// 模板文件夹
        /// </summary>
        public string TemplateFolder { get; set; }

        /// <summary>
        /// Bartend可执行文件路径
        /// </summary>
        public string BartendExePath { get; set; }

        /// <summary>
        /// 是否完成，True-完成，False-未完成
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// 最终生成的Excel文件
        /// </summary>
        public string GenerateExcelFile { get; set; }

        /// <summary>
        /// 最终生成的Pdf文件
        /// </summary>
        public string GeneratePdfFile { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
    }
}
