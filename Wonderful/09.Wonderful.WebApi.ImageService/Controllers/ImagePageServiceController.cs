using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wonderful.WebApi.ImageService.Models;

namespace Wonderful.WebApi.ImageService.Controllers
{
    /// <summary>
    /// 拍照页面服务
    /// </summary>
    public class ImagePageServiceController : Controller
    {
        /// <summary>
        /// 请求拍照任务页面
        /// </summary>
        /// <returns>拍照任务页面</returns>
        public IActionResult ImageCaptureTaskPage()
        {
            return View();
        }

        /// <summary>
        /// 请求拍照页面
        /// </summary>
        /// <returns>拍照页面</returns>
        public IActionResult ImageCapturePage([FromBody]ImageCapturePageRequest imageCapturePageRequest=null)
        {
            Uri location = new Uri($"{Request.Scheme}://{Request.Host}");
            string rootUrl = location.AbsoluteUri;
            rootUrl = rootUrl.Substring(0, rootUrl.Length - 1);
            ViewData["ImageCaptureSaveUrl"] = rootUrl + "/api/ImageService/SaveCaptureValue";
            ViewData["ImageCapturePageRequest"] = imageCapturePageRequest;
            return View();
        }
    }
}