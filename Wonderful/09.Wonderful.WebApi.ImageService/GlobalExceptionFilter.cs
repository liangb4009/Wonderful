using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _09.Wonderful.WebApi.ImageService
{
    /// <summary>
    /// 全局异常过滤
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            string msg = context.Exception.Message + context.Exception.StackTrace;
            context.Result = new ContentResult { Content = "{'Result':'false','Msg':'" + msg + "'}" };
        }
    }
}
