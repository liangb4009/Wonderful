using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wonderful.WebApi.ScaleService.Models;

namespace Wonderful.WebApi.ScaleService.Controllers
{
    public class ScalePageServiceController : Controller
    {
        /// <summary>
        /// 请求称重任务页面
        /// </summary>
        /// <returns>显示称重任务页面</returns>
        public IActionResult DisplayScalePage()
        {
   

            return View();
        }
        /// <summary>
        /// 请求称重页面
        /// </summary>
        /// <param name="scalePageRequest">称重页面请求</param>
        /// <returns>称重页面</returns>
        public IActionResult PostScalePage([FromBody]ScalePageRequest scalePageRequest = null)
        {
            Uri location = new Uri($"{Request.Scheme}://{Request.Host}");
            string rootUrl = location.AbsoluteUri;
            rootUrl = rootUrl.Substring(0, rootUrl.Length - 1);
            ViewData["ScaleSaveUrl"] = rootUrl + "/api/ScaleService/PostScaleSaveValue";
            ViewData["ScalePageRequest"] = scalePageRequest;
            return View();
        }
    }
}