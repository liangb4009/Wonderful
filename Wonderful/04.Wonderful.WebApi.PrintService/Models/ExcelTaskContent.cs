using System.Collections.Generic;

namespace Wonderful.WebApi.PrintService.Models
{
    /// <summary>
    /// Excel打印任务需要的内容
    /// </summary>
    public class ExcelTaskContent
    {
        /// <summary>
        /// 工作表名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 单元格内容
        /// </summary>
        public IEnumerable<NamedSubString> CellContent { get; set; }

        /// <summary>
        /// 表格内容
        /// </summary>
        public IEnumerable<TableContent> TableContent { get; set; }
    }

    /// <summary>
    /// 表格内容
    /// </summary>
    public class TableContent
    {
        /// <summary>
        /// 表格名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 具体内容
        /// </summary>
        public IEnumerable<object> Content { get; set; }
    }
}
