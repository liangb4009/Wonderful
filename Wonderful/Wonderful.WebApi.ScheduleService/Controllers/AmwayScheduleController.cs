using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32.TaskScheduler;
using Wonderful.Util.Helper;
using Wonderful.WebApi.ScheduleService.Enums;
using Wonderful.WebApi.ScheduleService.Models.Requests;

namespace Wonderful.WebApi.ScheduleService.Controllers
{
    /// <summary>
    /// 计划任务API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AmwayScheduleController : ControllerBase
    {

        public const string GlobalSchedulePath = @"\OA";

        public const string ApplicationSchedulePath = @"\OA\{0}";
        /// <summary>
        /// 执行计划任务API
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        [HttpPost("ExecuteTask")]
        public ActionResult ExecuteTask(ExecuteTaskRequest request)
        {
            string logContent = "";
            string errorMsg = null;
            string path;
            if (string.IsNullOrEmpty(request.SystemCode))
            {
                path = GlobalSchedulePath;
            }
            else
            {
              

                path = string.Format(ApplicationSchedulePath, request.SystemCode);
            }

            TaskService ts= default(TaskService);
            try
            {
                var scheduleServer = ConfigHelper.GetValue("ScheduleServer");
                if (string.IsNullOrEmpty(scheduleServer))
                {
                    ts = new TaskService();
                }
                else
                {

                    ts = new TaskService(scheduleServer);
                }
            }
            catch (Exception ex)
            {
                errorMsg = "初始化ScheduleService错误：" + ex.Message + ex.InnerException;
            }


            bool found = false;
            TaskFolder folder = ts.RootFolder;
            try
            {
                try
                {
                    folder = ts.GetFolder(path);
                    if (null != folder)
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                    }                   
                }
                catch (System.IO.FileNotFoundException)
                {
                    found = false;
                }
                logContent += ("found:" + found.ToString());
                if (found)
                {
                    if (folder.Tasks.Count(o => o.Name == request.ScheduleCode) > 0)
                    {                      
                        Task task = folder.Tasks.SingleOrDefault(o => o.Name == request.ScheduleCode);
                        task.Run();
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message + ex.InnerException;
            }
            finally
            {
                folder = null;
                ts = null;
            }
            //System.IO.File.WriteAllText("E:/txt.txt", contentStr);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return new JsonResult(new { Success = false, Msg = logContent, ErrorMsg = errorMsg });
            }
            return  new JsonResult(new{ Success=true, Msg = logContent});
        }
        /// <summary>
        /// 更新计划任务API
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        [HttpPost("UpdateSchedule")]
        public ActionResult UpdateSchedule(UpdateScheduleRequest request)
        {
            string logContent = "";
            string errorMsg = null;
            string path;
            if (string.IsNullOrEmpty(request.SystemCode))
            {
                path = GlobalSchedulePath;
            }
            else
            {            
                path = string.Format(ApplicationSchedulePath, request.SystemCode);
            }

            TaskService ts=default(TaskService);
            try
            {
                logContent += ("初始化ScheduleService");
                var scheduleServer = ConfigHelper.GetValue("ScheduleServer");
                if (string.IsNullOrEmpty(scheduleServer))
                {
                    ts = new TaskService();
                }
                else
                {
                    ts = new TaskService(scheduleServer);
                }
            }
            catch (Exception ex)
            {
                errorMsg = "初始化ScheduleService错误：" + ex.Message + ex.InnerException;
            }
            if (string.IsNullOrEmpty(errorMsg))
            {
                bool found = false;
                TaskFolder folder = ts.RootFolder;
                try
                {
                    try
                    {
                        folder = ts.GetFolder(path);
                        if (null != folder)
                        {
                            found = true;
                        }
                        else
                        {
                            found = false;
                        }
                    }
                    catch (Exception)
                    {
                        found = false;
                    }
                    logContent += ("found:" + found.ToString());
                    if (!found)
                    {
                        folder = ts.RootFolder.CreateFolder(path);
                    }

                    TaskDefinition td = ts.NewTask();

                    td.RegistrationInfo.Description = request.Descr;


                    Microsoft.Win32.TaskScheduler.TimeTrigger tr = new TimeTrigger();
                    if (!string.IsNullOrEmpty(request.StartDate))
                    {
                        tr.StartBoundary = DateTime.Parse(request.StartDate);
                    }

                    if (!string.IsNullOrEmpty(request.EndDate))
                    {
                        tr.EndBoundary = DateTime.Parse(request.EndDate);
                    }
                    tr.Repetition.Duration = new TimeSpan(0);
                    tr.Repetition.Interval = new TimeSpan(0, int.Parse(request.Period), 0);
                    tr.Enabled = true;
                    td.Triggers.Add(tr);

                    td.Actions.Add(new ExecAction(ConfigHelper.GetValue("TaskExecutorPath"), request.ScheduleId));

                    if (!string.IsNullOrEmpty(request.RetryCount))
                    {
                        var retryCountInt = int.Parse(request.RetryCount);
                        td.Settings.RestartCount = retryCountInt;
                        td.Settings.RestartInterval = new TimeSpan(0, 1, 0);
                    }

                    //if (folder.Tasks.Count(o => o.Name == scheduleObj.ScheduleCode) > 0)
                    //{
                    //    folder.DeleteTask(scheduleObj.ScheduleCode);
                    //}
                    //td.Principal.LogonType = TaskLogonType.Password;
                    //td.Principal.RunLevel = TaskRunLevel.Highest;
                    //td.Principal = 
                    //using (System.Web.Hosting.HostingEnvironment.Impersonate())
                    //{
                    td.Settings.Enabled = !string.IsNullOrEmpty(request.ScheduleStatus) && request.ScheduleStatus == ((int)EnumRecordStatus.Enabled).ToString();
                    //td.Settings. = false;
                    var ScheduleUser = ConfigHelper.GetValue("ScheduleUser");
                    var SchedulePassword = ConfigHelper.GetValue("SchedulePassword");
                    logContent += ("|ScheduleUser:" + ScheduleUser);
                    logContent += ("|SchedulePassword:" + SchedulePassword);
                    if (string.IsNullOrEmpty(ScheduleUser) || string.IsNullOrEmpty(SchedulePassword))
                    {
                        errorMsg = "web.config 中没有配置ScheduleUser 和 SchedulePassword";

                        //Task task = folder.RegisterTaskDefinition(scheduleObj.ScheduleCode, td, TaskCreation.Create, @"gzdemo-MSFT\gzdemo", "Pass@word1", TaskLogonType.Password, null);
                    }
                    else
                    {
                        Task task = folder.RegisterTaskDefinition(request.ScheduleCode, td, TaskCreation.CreateOrUpdate, ScheduleUser, SchedulePassword, TaskLogonType.Password, null);
                    }
                    logContent += "初始化成功";
                    //task.Definition.Principal.RunLevel = TaskRunLevel.Highest;
                    //folder.RegisterTaskDefinition(scheduleObj.ScheduleCode, task.Definition, TaskCreation.Update, @"gzdemo-MSFT\gzdemo", "Pass@word1", TaskLogonType.Password, null);
                    //Task task = folder.RegisterTaskDefinition(scheduleObj.ScheduleCode, td); 
                    //}

                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message + ex.InnerException;
                }
                finally
                {
                    folder = null;
                    ts = null;
                }

            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return new JsonResult(new { Success = false, Msg = logContent, ErrorMsg= errorMsg });
            }

            return new JsonResult(new { Success=true,Msg= logContent });

        }
      

    }
}