using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wonderful.WebApi.ScaleService.Models;

namespace Wonderful.WebApi.ScaleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScaleServiceController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly string TokenId = "00000000-0000-0000-0000-000000000001";
        private readonly string TaskId = "00000000-0000-0000-0000-000000000001";
        private readonly string ScaleType = "manual";
        private readonly string ScaleName = "HPC Barsoap Scale";
        private readonly ScaleParams scaleParams = new ScaleParams() { ScaleParamId=1, ScaleName = "HPC Barsoap Scale", ScalePort = "COM1", ScaleBitrate=9600, todoItem=null };
        private readonly int ScaleCount = 1;
        private readonly int ScaleFrequency = 1;
        private readonly List<ScaleValue> scaleValues = new List<ScaleValue>() {
            new ScaleValue{ ScaleValueId=1, TodoItem=null, Value=0, Name="1", DateTime=DateTime.Now.ToUniversalTime() }
        };
        /// <summary>
        /// 构造函数，默认增加一条初始化任务
        /// </summary>
        /// <param name="context">代办列表上下文</param>
        public ScaleServiceController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                TodoItem todoItem = new TodoItem
                {
                    TokenId = this.TokenId,
                    TaskId = this.TaskId,
                    ScaleType = this.ScaleType,
                    ScaleName = this.ScaleName,
                    ScaleCount = this.ScaleCount,
                    ScaleFrequency = this.ScaleFrequency,
                    IsComplete = true,
                    ScaleParams = this.scaleParams,
                    ScaleValues = this.scaleValues,
                };
                scaleValues.ForEach(m => m.TodoItem = todoItem);
                scaleValues.ForEach(m => _context.ScaleValues.Add(m));
                scaleParams.todoItem = todoItem;
                _context.ScaleParams.Add(scaleParams);
                _context.TodoItems.Add(todoItem);
                _context.SaveChanges();
            }
        }
        /// <summary>
        /// 请求称重
        /// POST: api/ScaleService/PostScale
        /// </summary>
        /// <param name="scaleRequest">请求称重请求</param>
        /// <returns>请求称重结果</returns>
        [EnableCors]
        [HttpPost("PostScale")]
        public ActionResult<ScaleResponse> PostScale(ScaleRequest scaleRequest)
        {
            //生成TaskId
            string TaskId = Guid.NewGuid().ToString();
            //生成TodoItem并保存
            ScaleParams scaleParams = this.scaleParams;
            List<ScaleValue> scaleValues = this.scaleValues;
            scaleParams.TaskId = TaskId;
            //主键不能重复
            scaleParams.ScaleParamId = _context.ScaleParams.Count() + 1;
            int currentCount = _context.ScaleValues.Count();
            scaleValues.ForEach(m => {
                m.ScaleValueId = currentCount + m.ScaleValueId;
            });
            //更新关联关系
            TodoItem todoItem = new TodoItem { TokenId = scaleRequest.TokenId, TaskId = TaskId, ScaleName = this.ScaleName, ScaleType = this.ScaleType, ScaleCount = this.ScaleCount, ScaleFrequency = this.ScaleFrequency, ScaleParams = scaleParams, ScaleValues = this.scaleValues, IsComplete = false };
            scaleParams.todoItem = todoItem;
            scaleValues.ForEach(m=> {
                m.TodoItem = todoItem;
            });
            //保存关联关系
            _context.TodoItems.Add(todoItem);
            _context.ScaleParams.Add(scaleParams);
            scaleValues.ForEach(m => _context.ScaleValues.Add(m));
            _context.SaveChanges();
            //返回结果
            ScaleResponse scaleResponse = new ScaleResponse { TokenId = scaleRequest.TokenId, TaskId = TaskId, Result = true, Msg = "" };
            return scaleResponse;
        }
        /// <summary>
        /// 保存称重任务
        /// POST: api/ScaleService/PostScaleSaveTask
        /// </summary>
        /// <param name="scaleSaveTask">请求，保存除ScaleValues外所有信息</param>
        /// <returns>响应</returns>
        [EnableCors]
        [HttpPost("PostScaleSaveTask")]
        public ActionResult<ScaleSaveTaskResponse> PostScaleSaveTask(ScaleSaveTaskRequest scaleSaveTask)
        {
            //返回结果
            ScaleSaveTaskResponse scaleSaveTaskResponse = new ScaleSaveTaskResponse { TaskId = scaleSaveTask.TaskId, Msg = "", Result = true };
            //获得TodoItem
            TodoItem item = _context.TodoItems.Find(scaleSaveTask.TaskId);
            //获得当前TaskId的电子秤参数
            ScaleParams scaleParams = _context.ScaleParams.FirstOrDefault(m => m.TaskId == scaleSaveTask.TaskId);
            //获得当前TaskId的秤重值
            List<ScaleValue> scaleValues = _context.ScaleValues.Where(s => s.TaskId == scaleSaveTask.TaskId).ToList();
            //默认TodoItem不为null
            if (item != null)
            {
                item.ScaleType = scaleSaveTask.ScaleType;
                item.ScaleName = scaleSaveTask.ScaleName;
                item.ScaleCount = scaleSaveTask.ScaleCount;
                //初始化的称重次数小于用户指定的称重次数，则初始化剩余的称重次数
                if (scaleValues.Count < item.ScaleCount)
                {
                    int currentCountAsName = scaleValues.Count();
                    int currentCount = _context.ScaleValues.Count();
                    for (int i = 1; i <= item.ScaleCount - scaleValues.Count; i++)
                    {
                        _context.ScaleValues.Add(new ScaleValue
                        {
                            TaskId = scaleSaveTask.TaskId,
                            DateTime = DateTime.Now.ToUniversalTime(),
                            Name = (currentCountAsName + i).ToString(),
                            ScaleValueId = (currentCount + i),
                            TodoItem = item,
                            Value = 0
                        });
                    }
                }
                item.ScaleFrequency = scaleSaveTask.ScaleFrequency;
                scaleParams.ScalePort = scaleSaveTask.ScaleParams.ScalePort;
                item.ScaleParams = scaleParams;
                //重建关联
                scaleParams.todoItem = item;
                scaleValues.ForEach(m => m.TodoItem = item);
                //重新保存
                _context.Entry(item).State = EntityState.Modified;
                ScaleParams scaleParams1 = _context.ScaleParams.Find(scaleParams.ScaleParamId);
                _context.Entry(scaleParams1).State = EntityState.Modified;
                scaleValues.ForEach(m =>
                {
                    ScaleValue scaleValue = _context.ScaleValues.Find(m.ScaleValueId);
                    _context.Entry(scaleValue).State = EntityState.Modified;
                });
                _context.SaveChanges();
            }
            else
            {
                scaleSaveTaskResponse.Msg = "Can not find todoitem";
                scaleSaveTaskResponse.Result = false;
            }
            //返回结果
            return scaleSaveTaskResponse;
        }
        /// <summary>
        /// 请求保存当前重量
        /// POST: api/ScaleService/PostScaleSaveValue
        /// </summary>
        /// <param name="scaleSaveValues">请求保存当前重量请求</param>
        /// <returns>请求保存当前重量结果</returns>
        [EnableCors]
        [HttpPost("PostScaleSaveValue")]
        public ActionResult<ScaleSaveValueResponse> PostScaleSaveValue(ScaleSaveValueRequest scaleSaveValue)
        {
            //返回结果
            ScaleSaveValueResponse scaleSaveValuesResponse = new ScaleSaveValueResponse { TaskId = scaleSaveValue.TaskId, Msg = "Success to save value", Result = true };
            string taskid = scaleSaveValue.TaskId;
            ScaleValueRequest scaleValueRequest = scaleSaveValue.ScaleValue;
            TodoItem todoItem = _context.TodoItems.Find(taskid);
            List<ScaleValue> scaleValues = _context.ScaleValues.Where(s => s.TaskId == taskid).ToList();
            ScaleValue curScaleValue = _context.ScaleValues.Find(scaleValueRequest.ScaleValueId);
            if (curScaleValue != null)
            {
                curScaleValue.DateTime = scaleValueRequest.DateTime;
                curScaleValue.Name = scaleValueRequest.Name;
                curScaleValue.ScaleValueId = scaleValueRequest.ScaleValueId;
                curScaleValue.TaskId = scaleValueRequest.TaskId;
                curScaleValue.TodoItem = todoItem;
                curScaleValue.Value = scaleValueRequest.Value;
                if (scaleValues.TrueForAll(s => { return s.Value != 0; })) todoItem.IsComplete = true;
                _context.Entry(todoItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Entry(curScaleValue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                scaleSaveValuesResponse.Result = false;
                scaleSaveValuesResponse.Msg = "Can not find ScaleValue";
            }
            return scaleSaveValuesResponse;
        }
        /// <summary>
        /// 获得代办列表
        /// POST: api/ScaleService/TodoItems
        /// </summary>
        /// <returns>待办列表</returns>
        [EnableCors]
        [HttpPost("TodoItems")]
        public ActionResult<IEnumerable<TodoItemResponse>> PostTodoItems()
        {

            List<TodoItemResponse> response = new List<TodoItemResponse>();
            _context.TodoItems.ToList().ForEach(
                m => {
                    ScaleParams scalParam = _context.ScaleParams.FirstOrDefault(s => s.TaskId == m.TaskId);
                    ScaleParamsResponse rspScaleParam = new ScaleParamsResponse {
                         TaskId = scalParam.TaskId,
                         ScaleParamId = scalParam.ScaleParamId,
                          ScaleBitrate = scalParam.ScaleBitrate,
                           ScaleName = scalParam.ScaleName,
                            ScalePort = scalParam.ScalePort
                    };
                    List<ScaleValueResponse> rspScaleValues = new List<ScaleValueResponse>();
                    _context.ScaleValues.Where(v => v.TaskId == m.TaskId).ToList().ForEach(v => rspScaleValues.Add(
                            new ScaleValueResponse() {
                               TaskId = v.TaskId,
                                Name = v.Name,
                                 DateTime = v.DateTime,
                                  ScaleValueId = v.ScaleValueId,
                                   Value = v.Value
                            }
                        ));
                    response.Add(new TodoItemResponse
                    {
                        TokenId = m.TokenId,
                        TaskId = m.TaskId,
                        ScaleType = m.ScaleType,
                        ScaleName = m.ScaleName,
                        ScaleCount = m.ScaleCount,
                        IsComplete = m.IsComplete,
                        ScaleFrequency = m.ScaleFrequency,
                        ScaleParams = rspScaleParam,
                        ScaleValues = rspScaleValues
                    });
                }
            );

            return response;
        }

        /// <summary>
        /// 获得代办列表某一具体项
        /// POST: api/ScaleService/TodoItem
        /// </summary>
        /// <param name="request">请求，包括TaskId</param>
        /// <returns>响应</returns>
        [EnableCors]
        [HttpPost("TodoItem")]
        public ActionResult<TodoItemResponse> PostTodoItem(TodoItemRequest request)
        {
            ScaleParams scaleParams = _context.ScaleParams.FirstOrDefault(p => p.TaskId == request.TaskId);
            ScaleParamsResponse scaleParamsResponse = new ScaleParamsResponse();
            scaleParamsResponse.TaskId = scaleParams.TaskId;
            scaleParamsResponse.ScalePort = scaleParams.ScalePort;
            scaleParamsResponse.ScaleParamId = scaleParams.ScaleParamId;
            scaleParamsResponse.ScaleName = scaleParams.ScaleName;
            scaleParamsResponse.ScaleBitrate = scaleParams.ScaleBitrate;

            List<ScaleValue> scaleValues = _context.ScaleValues.Where(v => v.TaskId == request.TaskId).ToList();
            List<ScaleValueResponse> scaleValueResponses = new List<ScaleValueResponse>();
            scaleValues.ForEach(v => {
                scaleValueResponses.Add(
                        new ScaleValueResponse() {
                             DateTime = v.DateTime,
                              Name = v.Name,
                               ScaleValueId = v.ScaleValueId,
                                TaskId = v.TaskId,
                                 Value = v.Value
                        }
                    );
            }) ;

            TodoItem todoitem = _context.TodoItems.Find(request.TaskId);
            TodoItemResponse response = new TodoItemResponse();
            response.TokenId = todoitem.TokenId;
            response.TaskId = todoitem.TaskId;
            response.IsComplete = todoitem.IsComplete;
            response.ScaleCount = todoitem.ScaleCount;
            response.ScaleFrequency = todoitem.ScaleFrequency;
            response.ScaleName = todoitem.ScaleName;
            response.ScaleType = todoitem.ScaleType;
            response.ScaleParams = scaleParamsResponse;
            response.ScaleValues = scaleValueResponses;
            return response;
        }


    }
}
