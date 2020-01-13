using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wonderful.WebApi.PrintService
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }

    /// <summary>
    /// 操作日志
    /// </summary>
    public class UserOperationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UserOperationException() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UserOperationException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UserOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
