using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.PrintService
{
    /// <summary>
    /// 
    /// </summary>
    public class InitRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public static ILoggerRepository LoggerRepository { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LogHelper
    {
        private static readonly ILog logerror = LogManager.GetLogger(InitRepository.LoggerRepository.Name, "logerror");
        private static readonly ILog loginfo = LogManager.GetLogger(InitRepository.LoggerRepository.Name, "loginfo");
        
        #region 全局异常错误记录持久化
        /// <summary>
        /// 全局异常错误记录持久化
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void ErrorLog(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【抛出信息】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
            logerror.Error(errorMsg);
        }
        #endregion

        #region 自定义操作记录
        /// <summary>
        /// 自定义操作记录，与仓储中的增删改的日志是记录同一张表
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void WriteLog(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【抛出信息】：{0} <br>【异常类型】：{1} <br>【异常信息】：{2} <br>【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("位置", "<strong style=\"color:red\">位置</strong>");
            logerror.Error(errorMsg);
        }
        #endregion
    }

}
