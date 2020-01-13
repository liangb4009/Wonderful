using System;
using System.Collections.Generic;
using System.Text;

namespace Wonderful.Console1
{
    /// <summary>
    /// 内部静态类，提供路径转换公用函数
    /// </summary>
    internal static class CommonConvertHelper
    {
        /// <summary>
        /// 获得以"\"结尾的路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>以"\"结尾的路径</returns>
        public static string getPath(this string path)
        {
            return (path.Substring(path.Length - 1, 1) == @"\") ? path : new StringBuilder(path).Append(@"\").ToString();
        }

    }
}
