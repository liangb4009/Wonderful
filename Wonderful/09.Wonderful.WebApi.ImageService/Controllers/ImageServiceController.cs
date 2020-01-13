using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using System.Net;
using Wonderful.WebApi.ImageService.Models;
using ZXing;
using ZXing.Common;

namespace Wonderful.WebApi.ImageService.Controllers
{
    /// <summary>
    /// 拍照服务
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImageServiceController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly string TokenId = "00000000-0000-0000-0000-000000000001";
        private readonly string TaskId = "00000000-0000-0000-0000-000000000001";
        private readonly string RunMode = "takephoto";
        private readonly string CameraType = "front camera";
        private readonly string MachineName = "CNWSGZISDL917";
        private readonly int CaptureCount = 1;
        private readonly int CaptureFrequency = 2;
        private readonly List<ImageValue> imageValues = new List<ImageValue>() {
            new ImageValue{  Id=1, TodoItem=null, Name="1", Value="", DateTime=DateTime.Now.ToUniversalTime()}
        };
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 构造函数，默认增加一条初始化任务
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        public ImageServiceController(TodoContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            if (_context.TodoItems.Count() == 0)
            {
                TodoItem todoItem = new TodoItem
                {
                    TokenId = this.TokenId,
                    TaskId = this.TaskId,
                    RunMode = this.RunMode,
                    CameraType = this.CameraType,
                    MachineName = this.MachineName,
                    CaptureCount = this.CaptureCount,
                    CaptureFrequency = this.CaptureFrequency,
                    ImageValues = this.imageValues,
                    IsComplete = true
                };
                this.imageValues.ForEach(i => i.TodoItem = todoItem);
                this.imageValues.ForEach(i => _context.ImageValues.Add(i));
                _context.TodoItems.Add(todoItem);
                _context.SaveChanges();
            }

            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 请求拍照
        /// </summary>
        /// <param name="imageCaptureRequest">拍照请求</param>
        /// <returns>拍照响应</returns>
        [EnableCors]
        [HttpPost("Capture")]
        public ActionResult<ImageCaptureResponse> PostImageCapture(ImageCaptureRequest imageCaptureRequest)
        {
            //生成TaskId
            string TaskId = Guid.NewGuid().ToString();
            //生成TodoItem并保存
            List<ImageValue> imageValues = this.imageValues;
            //主键不能重复
            int currentCount = _context.ImageValues.Count();
            imageValues.ForEach(i => {
                i.Id = currentCount + i.Id;
            });
            //更新关联关系
            TodoItem todoItem = new TodoItem
            {
                TokenId = imageCaptureRequest.TokenId,
                TaskId = TaskId,
                RunMode = this.RunMode,
                MachineName = this.MachineName,
                CameraType = this.CameraType,
                CaptureCount = this.CaptureCount,
                CaptureFrequency = this.CaptureFrequency,
                ImageValues = this.imageValues,
                IsComplete = false
            };
            imageValues.ForEach(i=> {
                i.TodoItem = todoItem;
            });
            //保存关联关系
            _context.TodoItems.Add(todoItem);
            imageValues.ForEach(i => _context.ImageValues.Add(i));
            _context.SaveChanges();
            //返回结果
            ImageCaptureResponse imageCaptureResponse = new ImageCaptureResponse {
                TokenId = imageCaptureRequest.TokenId,
                TaskId = TaskId,
                Result = true,
                Msg = ""
            };
            return imageCaptureResponse;
        }

        /// <summary>
        /// 请求保存拍照任务
        /// </summary>
        /// <param name="imageSaveCaptureTaskRequest">保存拍照任务请求</param>
        /// <returns>保存拍照任务响应</returns>
        [EnableCors]
        [HttpPost("SaveCaptureTask")]
        public ActionResult<ImageSaveCaptureTaskResponse> PostImageSaveCaptureTask(ImageSaveCaptureTaskRequest imageSaveCaptureTaskRequest)
        {
            //返回结果
            ImageSaveCaptureTaskResponse imageSaveCaptureTaskResponse = new ImageSaveCaptureTaskResponse { TaskId = imageSaveCaptureTaskRequest.TaskId, Result = true, Msg = ""};
            //获得TodoItem
            TodoItem item = _context.TodoItems.Find(imageSaveCaptureTaskRequest.TaskId);
            //获得当前TaskId的图像
            List<ImageValue> imageValues = _context.ImageValues.Where(i => i.TaskId == imageSaveCaptureTaskRequest.TaskId).ToList();
            //默认TodoItem不为null
            if (item != null)
            {
                item.RunMode = imageSaveCaptureTaskRequest.RunMode;
                item.MachineName = imageSaveCaptureTaskRequest.MachineName;
                if (string.IsNullOrWhiteSpace(item.MachineName))
                {
                    var ipAddress = Request.HttpContext.Connection.RemoteIpAddress;
                    var clientName = ipAddress.ToString();
                    try
                    {
                        var ipHostEntry = Dns.GetHostEntry(ipAddress);
                        clientName = ipHostEntry.HostName;
                    }
                    catch (Exception ex) { }
                    item.MachineName = clientName;
                }
                item.CameraType = imageSaveCaptureTaskRequest.CameraType;
                item.CaptureCount = imageSaveCaptureTaskRequest.CaptureCount;
                //初始化的拍照次数小于用户指定的拍照次数，则初始化剩余的拍照图像
                if (imageValues.Count < item.CaptureCount)
                {
                    int currentCountAsName = imageValues.Count();
                    int currentCount = _context.ImageValues.Count();
                    for (int i = 1; i <= item.CaptureCount - imageValues.Count; i++)
                    {
                        _context.ImageValues.Add(new ImageValue
                        {
                            TaskId = imageSaveCaptureTaskRequest.TaskId,
                            DateTime = DateTime.Now.ToUniversalTime(),
                            Name = (currentCountAsName + i).ToString(),
                            Value = "",
                            Id = (currentCount + i),
                            TodoItem = item
                        });
                    }
                }
                item.CaptureFrequency = imageSaveCaptureTaskRequest.CaptureFrequency;
                //重建关联
                imageValues.ForEach(i => i.TodoItem = item);
                //重新保存
                _context.Entry(item).State = EntityState.Modified;
                imageValues.ForEach(i =>
                {
                    ImageValue imageValue = _context.ImageValues.Find(i.Id);
                    _context.Entry(imageValue).State = EntityState.Modified;
                });
                _context.SaveChanges();
            }
            else {
                imageSaveCaptureTaskResponse.Msg = "Can not find todoitem";
                imageSaveCaptureTaskResponse.Result = false;
            }

            return imageSaveCaptureTaskResponse;
        }

        /// <summary>
        /// 保存当前摄像头图像
        /// </summary>
        /// <param name="imageSaveCaptureValueRequest">保存当前摄像头图像请求</param>
        /// <returns>保存当前摄像头图像响应</returns>
        [EnableCors]
        [HttpPost("SaveCaptureValue")]
        public ActionResult<ImageSaveCaptureValueResponse> PostImageSaveCaptureValue(ImageSaveCaptureValueRequest imageSaveCaptureValueRequest)
        {
            //返回结果
            ImageSaveCaptureValueResponse imageSaveCaptureValueResponse = new ImageSaveCaptureValueResponse { TaskId = imageSaveCaptureValueRequest.TaskId, Result = true, Msg = "Success to save value" };

            string taskid = imageSaveCaptureValueRequest.TaskId;
            TodoItem todoItem = _context.TodoItems.Find(taskid);
            if (todoItem == null)
            {
                imageSaveCaptureValueResponse.Result = false;
                imageSaveCaptureValueResponse.Msg = "Can not find todoItem";
                return imageSaveCaptureValueResponse;
            }
            List<ImageValue> imageValues = _context.ImageValues.Where(i => i.TaskId == taskid).ToList();
            if (todoItem.RunMode== "takephoto")
            {
                foreach (var item in imageSaveCaptureValueRequest.ImageValue)
                {
                    ImageValue imageValue = imageValues.Find(m => m.Id == item.Id);
                    if (imageValue != null)
                    {
                        imageValue.Id = item.Id;
                        imageValue.DateTime = item.DateTime;
                        imageValue.Name = item.Name;
                        imageValue.Value = UploadFile($"{item.Name}.jpg", item.Value, $"/Upload/Image/{item.TaskId}");
                        imageValue.TodoItem = todoItem;
                        imageValue.TaskId = item.TaskId;
                        if (imageValues.TrueForAll(i => { return i.Value != ""; })) todoItem.IsComplete = true;
                        _context.Entry(todoItem).State = EntityState.Modified;
                        _context.Entry(imageValue).State = EntityState.Modified;
                    }
                    else
                    {
                        imageSaveCaptureValueResponse.Result = false;
                        imageSaveCaptureValueResponse.Msg = "Can not find imageValue";
                        return imageSaveCaptureValueResponse;
                    }

                    _context.SaveChanges();
                }
            }
            else if (todoItem.RunMode == "scanqrcode")
            {
                ImageValueRequest imageValueRequest = imageSaveCaptureValueRequest.ImageValue[0];
                ImageValue imageValue = imageValues.Find(m => m.Id == imageValueRequest.Id);
                if (imageValue != null)
                {
                    var result = QRDecode(imageValueRequest.Value);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        imageSaveCaptureValueResponse.Msg = result;

                        imageValue.Id = imageValueRequest.Id;
                        imageValue.DateTime = imageValueRequest.DateTime;
                        imageValue.Name = imageValueRequest.Name;
                        imageValue.Value = result;
                        imageValue.TodoItem = todoItem;
                        imageValue.TaskId = imageValueRequest.TaskId;
                        if (imageValues.TrueForAll(i => { return i.Value != ""; })) todoItem.IsComplete = true;
                        _context.Entry(todoItem).State = EntityState.Modified;
                        _context.Entry(imageValue).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        imageSaveCaptureValueResponse.Result = false;
                        imageSaveCaptureValueResponse.Msg = "";
                    }
                }
                else
                {
                    imageSaveCaptureValueResponse.Result = false;
                    imageSaveCaptureValueResponse.Msg = "Can not find imageValue";
                }
            }

            return imageSaveCaptureValueResponse;
        }

        /// <summary>
        /// 获得所有待办项目
        /// </summary>
        /// <returns>所有待办项目</returns>
        [EnableCors]
        [HttpPost("TodoItems")]
        public ActionResult<IEnumerable<TodoItemResponse>> PostTodoItems()
        {
            List<TodoItemResponse> responses = new List<TodoItemResponse>();
            _context.TodoItems.OrderByDescending(m => m.CreateDate).ToList().ForEach(
                m =>
                {
                    List<ImageValueResponse> imageValueResponses = new List<ImageValueResponse>();
                    _context.ImageValues.Where(v => v.TaskId == m.TaskId).OrderBy(v => v.Id).ToList().ForEach(v => imageValueResponses.Add(
                              new ImageValueResponse()
                              {
                                  Id = v.Id,
                                  TaskId = v.TaskId,
                                  Name = v.Name,
                                  DateTime = v.DateTime,
                                  Value = v.Value
                              }
                          ));
                    responses.Add(new TodoItemResponse
                    {
                        TokenId = m.TokenId,
                        TaskId = m.TaskId,
                        RunMode = m.RunMode,
                        MachineName = m.MachineName,
                        CameraType = m.CameraType,
                        CaptureCount = m.CaptureCount,
                        CaptureFrequency = m.CaptureFrequency,
                        ImageValues = imageValueResponses,
                        IsCompleted = m.IsComplete
                    });
                });
            return responses;
        }

        /// <summary>
        /// 获得一个待办项目
        /// </summary>
        /// <param name="request">待办项目请求</param>
        /// <returns>待办项目</returns>
        [EnableCors]
        [HttpPost("TodoItem")]
        public ActionResult<TodoItemResponse> PostTodoItem(TodoItemRequest request)
        {
            List<ImageValue> imageValues = _context.ImageValues.Where(m => m.TaskId == request.TaskId).OrderBy(m => m.Id).ToList();
            List<ImageValueResponse> imageValueResponses = new List<ImageValueResponse>();
            imageValues.ForEach(v=> {
                imageValueResponses.Add(
                    new ImageValueResponse() {
                        Id = v.Id,
                        Name = v.Name,
                        Value = v.Value,
                        DateTime = v.DateTime,
                        TaskId = v.TaskId
                    }
                );
            });
            TodoItem todoItem = _context.TodoItems.Find(request.TaskId);
            TodoItemResponse todoItemResponse = new TodoItemResponse();
            todoItemResponse.TokenId = todoItem.TokenId;
            todoItemResponse.TaskId = todoItem.TaskId;
            todoItemResponse.RunMode = todoItem.RunMode;
            todoItemResponse.MachineName = todoItem.MachineName;
            todoItemResponse.CameraType = todoItem.CameraType;
            todoItemResponse.CaptureCount = todoItem.CaptureCount;
            todoItemResponse.CaptureFrequency = todoItem.CaptureFrequency;
            todoItemResponse.ImageValues = imageValueResponses;
            todoItemResponse.IsCompleted = todoItem.IsComplete;
            return todoItemResponse;
        }

        private string QRDecode(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);

            Bitmap bitmap = new Bitmap(new MemoryStream(bytes));

            var barcodeReader = new ZXing.ZKWeb.BarcodeReader();
            barcodeReader.Options = new DecodingOptions
            {
                CharacterSet = "UTF-8"
            };
            Result result = barcodeReader.Decode(bitmap);

            return result == null ? null : result.Text;
        }

        private string UploadFile(string fileName, string data, string filePath = "")
        {
            Uri location = new Uri($"{Request.Scheme}://{Request.Host}");
            string rootUrl = location.AbsoluteUri;
            rootUrl = rootUrl.Substring(0, rootUrl.Length - 1);
            if (data.Contains(rootUrl))
            {
                return data;
            }

            byte[] bytes = Convert.FromBase64String(data);
         
            string relativeDir = $"/Upload/Image";
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                relativeDir = filePath;
            }
            string absoluteDir = _hostingEnvironment.WebRootPath + relativeDir.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (!Directory.Exists(absoluteDir))
                Directory.CreateDirectory(absoluteDir);
            string file = Path.Combine(absoluteDir, fileName);
            using (FileStream fileStream = new FileStream(file, FileMode.Create))
            {
                using (MemoryStream m = new MemoryStream(bytes))
                {
                    m.WriteTo(fileStream);
                }
            }


            return $"{rootUrl}{relativeDir}/{fileName}";
        }
    }
}